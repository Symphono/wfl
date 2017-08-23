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
        callback(JSON.parse(body));
    });
}

function getFoodOrderById(id, callback)
{
    let options = {
        url: process.env.DB_URI + '/food-order/' + id
    }
    request.get(options, function(err, res, body){
        callback(JSON.parse(body));
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
    },
    getMenuSelectionTable: function(foodOrderId, callback) {
        getFoodOrderById(foodOrderId, function(json){
            if (json.entities)
            {
                console.log(json.entities);
                //Unfinished implementation
            }
        })
    },
    postMenuSelection: function(foodOrderId, name, description, callback) {
        let options = {
            url: process.env.DB_URI + '/food-order/' + foodOrderId + '/menu-selection',
            form: {
                OrdererName: name,
                Description: description
            }
        }

        request.post(options, function(err, res, body){
            let json = JSON.parse(body);
            callback(json.entities[json.entities.length - 1]);
        });
    },
    deleteMenuSelection: function(foodOrderId, menuSelectionId, callback) {
        let options = {
            url: process.env.DB_URI + '/food-order/' + foodOrderId + '/menu-selection/' + menuSelectionId,
        }
        request.delete(options, function(err, res, body){
            callback();
        });
    }
};