var Slackbot = require('slackbots');
var handleWflRequest = require('./handle-wfl-request')

var orderIdTable = {};
var menuSelectionIdTable = {};

var bot = new Slackbot({
    token: process.env.SLACK_BOT_TOKEN,
    name: "WFL bot"
});

bot.on('message', function(data) {
    if (data.text && data.subtype !== 'bot_message') {
        if (data.text.startsWith('wfl order create')) {
            handleWflRequest.handleOrderCreateRequest(bot, data, orderIdTable);
        }
        else if (data.text.startsWith('wfl order discard')) {
            handleWflRequest.handleOrderDiscardRequest(bot, data, orderIdTable);
        }
        else if (data.text.startsWith('wfl order complete')) {
            handleWflRequest.handleOrderCompleteRequest(bot, data, orderIdTable);
        }
        else if (data.text.startsWith('wfl gimme')) {
            handleWflRequest.handleGimmeRequest(bot, data, orderIdTable, menuSelectionIdTable)
        }
        else if (data.text.startsWith('wfl')) {
            handleWflRequest.handleWflRequest(bot, data, orderIdTable);
        }
    }
});