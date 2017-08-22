var request = require('request');

function getRestaurantIdByName(name, callback) {
    let options = {
        url: process.env.DB_URI + '/restaurant',
        form: {
            Name: name
        }
    }

    request.get(options, function(err, res, body) {
        let json = JSON.parse(body);
        if(json.entities[0]) {
            callback(json.entities[0].properties.Id);
        }
    });
}

function postFoodOrder(restaurantId, callback) {
    let options = {
        url: process.env.DB_URI + '/food-order',
        form: {
            RestaurantId: restaurantId
        }
    }

    request.post(options, function(err, res, body) {
        let json = JSON.parse(body);
        if (json) {
            callback(json.properties.Id);
        }
    });
}

function getRestaurants(callback)
{
    let options = {
        url: process.env.DB_URI + '/restaurant'
    }

    request.get(options, function(err, res, body) {
        let json = JSON.parse(body);
        callback(json);
    });
}

module.exports = {
    createFoodOrderFromRestaurantName: function(name, callback) {
        getRestaurantIdByName(name, function(id) {
            postFoodOrder(id, function(id) {
                callback(id);
            })
        })
    },
    getArrayOfRestaurantNames: function(callback) {
        getRestaurants(function(json) {
            var names = [];
            if (json.entities)
            {
                for (let restaurant of json.entities)
                {
                    names.push(restaurant.properties.Name);
                }
                callback(names);
            }
        });
    }
};