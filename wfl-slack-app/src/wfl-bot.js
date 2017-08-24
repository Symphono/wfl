var Slackbot = require('slackbots');
var request = require('request');
var handleWflRequest = require('./handle-wfl-request')

var orderIdTable = {};
var menuSelectionIdTable = {};

var bot = new Slackbot({
    token: process.env.SLACK_BOT_TOKEN,
    name: "WFL bot"
});

bot.on('message', function(data) {
    if (data.text && data.subtype !== 'bot_message') {
        if (data.text.indexOf('wfl order create') != -1) {
            handleWflRequest.handleOrderCreateRequest(bot, data, orderIdTable);
        }
        else if (data.text.indexOf('wfl order discard') != -1) {
            handleWflRequest.handleOrderDiscardRequest(bot, data, orderIdTable);
        }
        else if (data.text.indexOf('wfl order complete') != -1) {
            handleWflRequest.handleOrderCompleteRequest(bot, data, orderIdTable);
        }
        else if (data.text.indexOf('wfl gimme') != -1) {
            handleWflRequest.handleGimmeRequest(bot, data, orderIdTable, menuSelectionIdTable)
        }
        else if (data.text.indexOf('wfl') != -1) {
            handleWflRequest.handleWflRequest(bot, data, orderIdTable);
        }
    }
});