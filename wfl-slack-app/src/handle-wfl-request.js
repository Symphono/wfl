var wflApi = require('./wfl-api')
var slackApi = require('./slack-api')

function createMenuSelection(bot, data, orderIdTable, menuSelectionIdTable, description)
{
    return slackApi
        .getUserInfo(data.user)
        .then(name => wflApi.postMenuSelection(orderIdTable[data.channel], name, description, function(selectionJson) {
            menuSelectionIdTable[data.user] = selectionJson.properties.Id;
            var message = 'OK! Your selection is ' + description + '.';
            bot.postMessage(data.channel, message);
        }))
}

function deleteMenuSelection(bot, data, orderIdTable, menuSelectionIdTable)
{
    wflApi.deleteMenuSelection(orderIdTable[data.channel], menuSelectionIdTable[data.user], function(){
        bot.postMessage(data.channel, 'Your selection has been deleted.');
    });
}

function isNullOrWhitespace(input) {
    if (typeof input === 'undefined' || input == null)
        return true;
    return input.replace(/\s/g, '').length < 1;
}

module.exports = {
    handleCreateOrderRequest: function(bot, data, orderIdTable) {
        if(orderIdTable[data.channel])
        {
            bot.postMessage(data.channel, 'Sorry, there is already an active order for this channel. Please discard or complete the current order before creating a new one.');
        }
        else
        {
            var restaurantName = data.text.substring(data.text.indexOf('wfl order create') + 17);
            wflApi.createFoodOrderFromRestaurantName(restaurantName, function(id) {
                orderIdTable[data.channel] = id;
                bot.postMessage(data.channel, 'OK, your food order has been created. You may now add menu selections!');
            });
        }
    },
    handleWflRequest: function(bot, data, orderIdTable) {
        if (orderIdTable[data.channel])
        {
            bot.postMessage(data.channel, 'There is currently an active order for this channel.');
        }
        else
        {
            wflApi.getArrayOfRestaurantNames(function(names) {
                var message = 'This channel does not have an active order. Here is a list of restaurants you may want to choose from!\n';
                for (let name of names)
                {
                    message = message + name + '\n';
                }
                bot.postMessage(data.channel, message);
            })
        }
    },
    handleGimmeRequest: function(bot, data, orderIdTable, menuSelectionIdTable) {
        if(orderIdTable[data.channel])
        {
            var description = data.text.substring(data.text.indexOf('wfl gimme') + 10);
            if (isNullOrWhitespace(description) && menuSelectionIdTable[data.user])
                deleteMenuSelection(bot, data, orderIdTable, menuSelectionIdTable);
            else if (!isNullOrWhitespace(description))
            {
                createMenuSelection(bot, data, orderIdTable, menuSelectionIdTable, description);
            }
            else
                bot.postMessage(data.channel, 'Please enter a description for your menu selection.');
        }
        else
        {
            bot.postMessage(data.channel, 'This channel does not have an active order.');
        }
    },
    handleListCurrentMenuSelectionsRequest: function(bot, data, orderIdTable) {
        if (orderIdTable[data.channel])
        {
            bot.postMessage(data.channel, 'These are the selections for the current order.');
            wflApi.getMenuSelectionTable(orderIdTable[data.channel], function(menuSelectionTable){

            });
        }
        else
        {
            bot.postMessage(data.channel, 'This channel does not have an active order.');
        }
    }
}