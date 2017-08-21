var Slackbot = require('slackbots');
var request = require('request');
var requestApi = require('./request-api')

var bot = new Slackbot({
    token: process.env.SLACK_BOT_TOKEN,
    name: "WFL bot"
});

bot.on('start', function() {
    var params = {
        icon_emoji: ':wave:'
    };
    bot.postMessageToUser('tnagaraja', 'wfl started up', params)
})

bot.on('message', function(data) {
    if (data.text)
    {
        if (data.text.indexOf('wfl order create') != -1)
        {
            //create food order if none already exists
        }
        else if (data.text == 'log restaurants')
        {
            requestApi.logRestaurants();
        }
    }
});