var Slackbot = require('slackbots');
var request = require('request');
var requestApi = require('./request-api')

var orderIdTable = {};

var bot = new Slackbot({
    token: process.env.SLACK_BOT_TOKEN,
    name: "WFL bot"
});

bot.on('message', function(data) {
    if (data.text)
    {
        if (data.text.indexOf('wfl order create') != -1)
        {
            if(orderIdTable[data.channel])
            {
                bot.postMessage(data.channel, 'Sorry, there is already an active order for this channel. Please discard or complete the current order before creating a new one.');
            }
            else
            {
                var restaurantName = data.text.substring(data.text.indexOf('wfl order create') + 17);
                requestApi.createFoodOrderFromRestaurantName(restaurantName, function(id) {
                    orderIdTable[data.channel] = id;
                    bot.postMessage(data.channel, 'OK, your food order has been created. You may now add menu selections!');
                });
            }
        }
        else if (data.text.indexOf('wfl') != -1)
        {
            if (orderIdTable[data.channel])
            {
                bot.postMessage(data.channel, 'There is currently an active order for this channel.');
            }
            else
            {
                requestApi.getArrayOfRestaurantNames(function(names) {
                    var message = 'This channel does not have an active order. Here is a list of restaurants you may want to choose from!\n';
                    for (let name of names)
                    {
                        message = message + name + '\n';
                    }
                    bot.postMessage(data.channel, message);
                })
            }
        }
    }
});