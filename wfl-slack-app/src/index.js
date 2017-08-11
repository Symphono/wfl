var bodyParser = require('body-parser');
var app = require('express')();

app.use(bodyParser.urlencoded({ extended: true}));
app.listen(process.env.PORT);

app.post('/wfl', (req, res) => {
    res.send();
});