var Slackbot = require('slackbots');
var request = require('request');
var orderManager = require('./handle-wfl-request')

var orderIdTable = {};
var menuSelectionIdTable = {};

var bot = new Slackbot({
    token: process.env.SLACK_BOT_TOKEN,
    name: "WFL bot"
});

bot.on('message', function(data) {
    if (data.text)
    {
        if (data.text.indexOf('wfl order create') != -1)
        {
            orderManager.handleCreateOrderRequest(bot, data, orderIdTable);
        }
        else if (data.text.indexOf('wfl selections') != -1)
        {
            orderManager.handleListCurrentMenuSelectionsRequest(bot, data, orderIdTable);
        }
        else if (data.text.indexOf('wfl gimme') != -1)
        {
            orderManager.handleGimmeRequest(bot, data, orderIdTable, menuSelectionIdTable)
        }
        else if (data.text.indexOf('wfl') != -1)
        {
            orderManager.handleWflRequest(bot, data, orderIdTable);
        }
    }
});