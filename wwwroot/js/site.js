$(document).ready(function () {
    var Starters = 0;
    var Dishes = 0;
    var DishesFish = 0;
    var Desserts = 0;
    var Sandwichs = 0;
    var SandwichsFish = 0;
    var Drinks = 0;
    var Cultery = 0;
    var Breads = 0;

    var pricePosition = $("div#price").offset();

    // Sélection d'un item.
    $(".list-item").click(function (e) {
        if ($(e.target).hasClass("selectable"))
            return false;

        var del = false;
        if ($(e.target).is(".right") || $(e.target).is(".delete"))
            del = true;

        if (del) {
            removePrice(this);
            removeItem(this);
        } else {
            addPrice(this);
            addItem(this);
            addMultiChoice(this);
        }
    });

    // Désélection du prix.
    $(".list-item").contextmenu(function (e) {
        e.preventDefault();
        removePrice(this);
        removeItem(this);
        removeMultiChoice(this);
    });

    function addItem(element) {
        var span = $(element).find("span.number");
        var txt = span.text();
        var nb = 0;
        if (txt !== "") nb = parseInt(txt);
        span.text(nb + 1);
        $(element).addClass("active");
    }

    function removeItem(element) {
        var span = $(element).find("span.number");
        var txt = span.text();
        var nb = 0;
        if (txt !== "") nb = parseInt(txt) - 1;
        if (nb <= 0) {
            span.text("");
            $(element).removeClass("active");
        } else span.text(nb);
    }

    function addPrice(element) {
        var data = $(element).attr("data-value");

        if (data === "starter")
            Starters += 1;
        if (data === "dish")
            Dishes += 1;
        if (data === "dish-fish")
            DishesFish += 1;
        if (data === "dessert")
            Desserts += 1;
        if (data === "sandwich")
            Sandwichs += 1;
        if (data === "sandwich-fish")
            SandwichsFish += 1;
        if (data === "drink")
            Drinks += 1;
        if (data === "extra-bread")
            Breads += 1;
        if (data === "extra-cultery")
            Cultery += 1;

        var price = getPrice();
        $("#price #value").text(price.toFixed(2));
    }

    function removePrice(element) {
        var id = $(element).attr("data-id");
        var data = $(element).attr("data-value");
        var nb = parseInt($("#" + id).text());

        if (isNaN(nb)) return;

        if (nb > 0) {
            if (data === "starter")
                Starters -= 1;
            if (data === "dish")
                Dishes -= 1;
            if (data === "dish-fish")
                DishesFish -= 1;
            if (data === "dessert")
                Desserts -= 1;
            if (data === "sandwich")
                Sandwichs -= 1;
            if (data === "sandwich-fish")
                SandwichsFish -= 1;
            if (data === "drink")
                Drinks -= 1;
            if (data === "extra-bread")
                Breads -= 1;
            if (data === "extra-cultery")
                Cultery -= 1;

            var price = getPrice();
            price > 0 ? $("#price #value").text(price.toFixed(2)) : $("#price #value").text(0);
        }
        else $("#price #value").text("0");
    }

    function getPrice() {
        var starters = Starters;
        var dishes = Dishes;
        var dishesFish = DishesFish;
        var desserts = Desserts;
        var sandwichs = Sandwichs;
        var sandwichsFish = SandwichsFish;
        var drinks = Drinks;
        var cultery = Cultery;
        var breads = Breads;

        var priceFull = 8;
        var priceMenu = 6.5;
        var priceMenuFish = 7;
        var priceStarter = 2;
        var priceDish = 4.7;
        var priceDishFish = 5;
        var priceDessert = 2;
        var priceSand = 3;
        var priceSandFish = 3.5;
        var priceSandMenu = 5.5;
        var priceSandMenuFish = 6;
        var priceDrink = 1.1;
        var priceBread = 0.3;
        var priceCultery = 0.2;

        var price = 0;
        var minus = 0;
        if (starters > 0 && dishes > 0 && desserts > 0) {
            minus = getMinus(starters, dishes, desserts);
            price += (minus * priceFull);
            starters -= minus;
            dishes -= minus;
            desserts -= minus;
        }
        if (starters > 0 && dishesFish > 0 && desserts > 0) {
            minus = getMinus(starters, dishesFish, desserts);
            price += (minus * priceFull);
            starters -= minus;
            dishesFish -= minus;
            desserts -= minus;
        }
        if (starters > 0 && dishes > 0) {
            minus = getMinus(starters, dishes);
            price += (minus * priceMenu);
            starters -= minus;
            dishes -= minus;
        }
        if (starters > 0 && dishesFish > 0) {
            minus = getMinus(starters, dishesFish);
            price += (minus * priceMenuFish);
            starters -= minus;
            dishesFish -= minus;
        }
        if (dishes > 0 && desserts > 0) {
            minus = getMinus(dishes, desserts);
            price += (minus * priceMenu);
            dishes -= minus;
            desserts -= minus;
        }
        if (dishesFish > 0 && desserts > 0) {
            minus = getMinus(dishesFish, desserts);
            price += (minus * priceMenuFish);
            dishesFish -= minus;
            desserts -= minus;
        }
        if (sandwichs > 0 && desserts > 0 && drinks > 0) {
            minus = getMinus(sandwichs, desserts, drinks);
            price += (minus * priceSandMenu);
            sandwichs -= minus;
            desserts -= minus;
            drinks -= minus;
        }
        if (sandwichsFish > 0 && desserts > 0 && drinks > 0) {
            minus = getMinus(sandwichsFish, desserts, drinks);
            price += (minus * priceSandMenuFish);
            sandwichsFish -= minus;
            desserts -= minus;
            drinks -= minus;
        }
        if (starters > 0)
            price += (starters * priceStarter);
        if (dishes > 0)
            price += (dishes * priceDish);
        if (dishesFish > 0)
            price += (dishesFish * priceDishFish);
        if (desserts > 0)
            price += (desserts * priceDessert);
        if (sandwichs > 0)
            price += (sandwichs * priceSand);
        if (sandwichsFish > 0)
            price += (sandwichsFish * priceSandFish);
        if (drinks > 0)
            price += (drinks * priceDrink);
        if (cultery > 0)
            price += (cultery * priceCultery);
        if (breads > 0)
            price += (breads * priceBread);

        return price;
    }

    function getMinus() {
        var minus = 0;
        for (var i = 0; i < arguments.length; i += 1) {
            if (i === 0 || minus > arguments[i])
                minus = arguments[i];
        }
        return minus;
    }

    function addMultiChoice(e) {
        var nb = $(e).find(".number").text();
        var multichoices = $(e).find(".multichoice");
        $.each(multichoices, function () {
            var selectables = $(this).find(".selectable");
            if (selectables.length > 0 && nb <= 1)
                $(selectables[0]).addClass("selected");
        });
    }

    function removeMultiChoice(e) {
        var nb = $(e).find(".number").text();
        var multichoices = $(e).find(".multichoice");
        $.each(multichoices, function () {
            var selectables = $(this).find(".selectable");
            $.each(selectables, function () {
                if (nb === "") $(this).removeClass("selected");
            });
        });
    }

    // plat_id - nb;
    // plat_id-nb - nb_of_chooses - first_choice - ...;
    function getChoices() {
        var str = "";
        var idx = 0;
        var choices = $(".choice");
        $.each(choices, function () {
            var id = $(this).data("id");
            var nb = $(this).find("span#" + id).text();

            if (nb.match(/\d+/)) {
                if (idx !== 0) str += ";";
                idx += 1;

                nb = parseInt(nb);
                str += id + "-" + nb;

                var multichoices = $(this).find(".multichoice");
                if (multichoices.length > 0) {
                    var selected = $(this).find(".selected");

                    if (selected.length > 0)
                        str += "-" + selected.length;

                    $.each(selected, function () {
                        var id = $(this).data("id");
                        str += "-" + id;
                    });
                }
            }
        });
        return str;
    }

    $("#mail").on("focusin mouseover", function () {
        $("#order #name").addClass("focus");
    });

    $("#mail").on("focusout mouseleave", function () {
        if (!$(this).is(":focus"))
            $("#order #name").removeClass("focus");
    });

    $("#send").on("click", function () {
        if ($(this).hasClass("disable"))
            return;

        $(this).addClass("disable");
        var choices = getChoices();
        var email = $('#order #mail').val();
        $.ajax({
            type: "POST",
            url: baseUrl + "Home/Send",
            data: { "choices": choices, "email": email },
            success: function (data) { window.location.href = data; }
        });
    });

    // Choix multiple pour les burgers
    $(".selectable").on("click", function () {
        var multichoice = $(this).parent();
        var selectables = $(multichoice).find(".selectable");
        var nb = parseInt($(this).closest(".choice").find(".number").text());

        var choice = $(this);
        $.each(selectables, function () {
            var actual = $(this);
            if (choice[0].innerText === actual[0].innerText && nb > 0) {
                if (choice.hasClass("selected"))
                    choice.removeClass("selected");
                else choice.addClass("selected");
            } else actual.removeClass("selected");
        });
    });

    $(window).scroll(function () {
        if ($(window).scrollTop() >= pricePosition.top && $(window).width() <= 767)
            $(".price-bar").addClass("price-bar-fixed");
        else
            $(".price-bar").removeClass("price-bar-fixed");
    });

    $("#btn-connection").click(function () {
        $.ajax({
            type: "GET",
            url: baseUrl + "Account/Account",
            success: function (e) {
                $("#connection").html(e);
            }
        });
    });
});