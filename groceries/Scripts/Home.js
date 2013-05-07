
var Home = {};

Home.InitIndex = function () {
    $("#LinkRemoveItem").die('click').live('click', function () {
        $("#FormIndex").submit();
    });

    $("span.imgtoggle").die('click').live('click', function () {
        $("img.on", $(this)).toggle();
        $("img.off", $(this)).toggle();
    });

};     

Home.InitAddProduct = function () {
    $("#LinkAddProduct").die('click').live('click', function () {
        $("#ProductForm").submit();
    });
};

