var request = require('request');
var Siren = require('super-siren').default;

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
            callback(json);
        }
    });
}

function getAllRestaurants(callback) {
    return Siren.get(process.env.DB_URI + '/restaurant').then(res => res.body.entities);
}

function getFoodOrderById(id, callback) {
    let options = {
        url: process.env.DB_URI + '/food-order/' + id
    }
    request.get(options, function(err, res, body){
        callback(JSON.parse(body));
    });
}

function getRestaurantByName(name, callback) {
    Siren.get(process.env.DB_URI + '/restaurant').then(res => res.body.findActionByName('filter-restaurants').perform({
        Name: name
    })).then(res => {
        console.log(res.body);
    });

    let options = {
        url: process.env.DB_URI + '/restaurant' + '?Name=' + name,
    }

    request.get(options, function(err, res, body) {
        let json = JSON.parse(body);
        if(json.entities && json.entities[0]) {
            callback(json.entities[0]);
        }
        else {
            callback();
        }
    });
}

module.exports = {
    getRestaurantByName: getRestaurantByName,
    postFoodOrder: postFoodOrder,
    getAllRestaurants: getAllRestaurants,
    getFoodOrderById: getFoodOrderById,
    postMenuSelection: function(foodOrderId, name, description, callback) {
        let options = {
            url: process.env.DB_URI + '/food-order/' + foodOrderId + '/menu-selection',
            form: {
                OrdererName: name,
                Description: description
            }
        }
        request.post(options, function(err, res, body) {
            let json = JSON.parse(body);
            callback(json.entities[json.entities.length - 1]);
        });
    },
    deleteMenuSelection: function(foodOrderId, menuSelectionId, callback) {
        let options = {
            url: process.env.DB_URI + '/food-order/' + foodOrderId + '/menu-selection/' + menuSelectionId,
        }
        request.delete(options, function(err, res, body) {
            callback();
        });
    },
    getRestaurantById: function(id, callback) {
        let options = {
            url: process.env.DB_URI + '/restaurant/' + id
        }
        request.get(options, function(err, res, body) {
            callback(JSON.parse(body));
        });
    },
    setFoodOrderStatus: function(id, status, callback) {
        let options = {
            url: process.env.DB_URI + '/food-order/' + id,
            form: {
                Status: status
            }
        }
        request.post(options, function(err, res, body) {
            callback(JSON.parse(body));
        });
    }
};