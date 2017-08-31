var wflApi = require('./wfl-api')
var slackApi = require('./slack-api')

var Status = {
    ACTIVE: 1,
    DISCARDED: 2,
    COMPLETED: 3
}

function getOrderDetails(bot, data, foodOrderJson) {
    wflApi.getRestaurantById(foodOrderJson.properties.RestaurantId, function(restaurantJson) {
        var message = 'Order Details:\n';
        let restaurantName = restaurantJson.properties.Name;
        message = message + 'Restaurant: ' + restaurantName + '\n';
        if (foodOrderJson.entities) {
            for (let selection of foodOrderJson.entities)
            {
                message = message + selection.properties.OrdererName + ': ' + selection.properties.Description + '\n';
            }
        }
        bot.postMessage(data.channel, message);
    });
}

function verifyAndSetStatus(bot, data, orderIdTable, status) {
    verifyOrderIsActiveAndContinue(data, orderIdTable, function(activeOrderExists, foodOrderJson) {
        if (activeOrderExists) {
            wflApi.setFoodOrderStatus(orderIdTable[data.channel], status, function(responseJson) {
                var actionCompleted = '';
                if (status === Status.DISCARDED) {
                    actionCompleted = 'discarded';
                }
                else if (status === Status.COMPLETED) {
                    actionCompleted = 'completed';
                }
                bot.postMessage(data.channel, 'The order has been ' + actionCompleted + '.');
            });
        }
        else {
            bot.postMessage(data.channel, 'This channel does not have an active order.');
        }
    });
}

function createMenuSelection(bot, data, orderIdTable, menuSelectionIdTable, description) {
    if(menuSelectionIdTable[data.user])
    {
        wflApi.deleteMenuSelection(orderIdTable[data.channel], menuSelectionIdTable[data.user], function(){});
    }
    return slackApi
        .getUserInfo(data.user)
        .then(user => wflApi.postMenuSelection(orderIdTable[data.channel], user.real_name, description, function(selectionJson) {
            menuSelectionIdTable[data.user] = selectionJson.properties.Id;
            var message = 'OK! ' + user.real_name + `'s` + ' selection is ' + description + '.';
            bot.postMessage(data.channel, message);
        }))
}

function deleteMenuSelection(bot, data, orderIdTable, menuSelectionIdTable) {
    wflApi.deleteMenuSelection(orderIdTable[data.channel], menuSelectionIdTable[data.user], function() {
        slackApi.getUserInfo(data.user)
        .then(user => bot.postMessage(data.channel, user.real_name + `'s` + ' selection has been deleted.'))
    });
}

function isNullOrWhitespace(input) {
    if (typeof input === 'undefined' || input == null)
        return true;
    return input.replace(/\s/g, '').length < 1;
}

function verifyOrderIsActiveAndContinue(data, orderIdTable, callback) {
    if (orderIdTable[data.channel])
    {
        var returnValue;
        wflApi.getFoodOrderById(orderIdTable[data.channel], function(foodOrderJson) {
            if (foodOrderJson.properties.Status == 1)
            {
                callback(true, foodOrderJson);
            }
            else
                callback(false);
        });
    }
    else
      callback(false);
}

module.exports = {
    handleOrderCreateRequest: function(bot, data, orderIdTable) {
        verifyOrderIsActiveAndContinue(data,orderIdTable, function(activeOrderExists) {
            if (activeOrderExists)
            {
                bot.postMessage(data.channel, 'Sorry, there is already an active order for this channel. Please discard or complete the current order before creating a new one.');
            }
            else
            {
                var restaurantName = data.text.substring(data.text.indexOf('wfl order create') + 17);
                wflApi.getRestaurantByName(restaurantName, function(restaurantJson) {
                    if (restaurantJson)
                    {
                        wflApi.postFoodOrder(restaurantJson.properties.Id, function(foodOrderJson){
                            orderIdTable[data.channel] = foodOrderJson.properties.Id;
                            bot.postMessage(data.channel, `OK, the food order has been created. You may now add menu selections using 'wfl gimme &lt; description &gt;'!`);
                        })
                    }
                    else
                        bot.postMessage(data.channel, 'The restaurant entered does not exist.')
                });
            }
        });
    },
    handleWflRequest: function(bot, data, orderIdTable) {
        verifyOrderIsActiveAndContinue(data, orderIdTable, function(activeOrderExists, foodOrderJson) {
            if (activeOrderExists)
            {
                getOrderDetails(bot, data, foodOrderJson);
            }
            else
            {
                wflApi.getAllRestaurants().then(entities => {
                    var message = 'This channel does not have an active order. Here is a list of restaurants you may want to choose from!\n';
                    entities.forEach(embeddedEntity => message = message + embeddedEntity.entity.properties.first() + '\n');
                    bot.postMessage(data.channel, message);
                });
            }
        });
    },
    handleGimmeRequest: function(bot, data, orderIdTable, menuSelectionIdTable) {
        verifyOrderIsActiveAndContinue(data, orderIdTable, function(activeOrderExists) {
            if (activeOrderExists) {
                var description = data.text.substring(data.text.indexOf('wfl gimme') + 10);
                if (isNullOrWhitespace(description) && menuSelectionIdTable[data.user])
                    deleteMenuSelection(bot, data, orderIdTable, menuSelectionIdTable);
                else if (!isNullOrWhitespace(description))
                    createMenuSelection(bot, data, orderIdTable, menuSelectionIdTable, description);
                else
                    bot.postMessage(data.channel, 'Please enter a description for your menu selection.');
            }
            else {
                bot.postMessage(data.channel, 'This channel does not have an active order.');
            }
        })
    },
    handleOrderDiscardRequest: function(bot, data, orderIdTable) {
        verifyAndSetStatus(bot, data, orderIdTable, Status.DISCARDED);
    },
    handleOrderCompleteRequest: function(bot, data, orderIdTable) {
        verifyAndSetStatus(bot, data, orderIdTable, Status.COMPLETED);
    }
}