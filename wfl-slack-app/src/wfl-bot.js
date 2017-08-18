var Slackbot = require('slackbots')

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