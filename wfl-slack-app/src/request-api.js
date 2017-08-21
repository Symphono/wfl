var request = require('request');

module.exports = {
    logRestaurants: function(id) {
        request(process.env.DB_URI + '/restaurant', function(err, res, body) {
            let json = JSON.parse(body);
            console.log(json.entities);
        });
    }
};