// o.Type is being set in Crudere.cs to the entity's type name (lower case)

//removes the item from the html
function del(o) {
    jQuery('#' + o.Type + o.Id).fadeOut(300, function () { jQuery(this).remove(); });
    if (o.Type == "promotion") refreshPromotionsGrid();
}

//update the item in html
function edit(o) {
    jQuery('#' + o.Type + o.Id).fadeOut(300, function () {
        jQuery(this).after(jQuery.trim(o.Content)).remove();
        if (o.Type == "product") adjustProducts();
    });
    
    if (o.Type == "promotion") refreshPromotionsGrid();
    
    location.reload();
}

//append the html for the created country
function createCountry(o) {
    jQuery('#Countries').prepend(o.Content);
    jQuery('#Countries .awe-li:first').hide().fadeIn();
}



//append the html for the created Ad
function createAd(o) {
    jQuery('#Ads').prepend(o.Content);
    jQuery('#Ads .awe-li:first').hide().fadeIn();
}

//append the html for the created product
function createProduct(o) {
    jQuery('#Products').parent().find('ul').prepend(jQuery.trim(o.Content));
    location.reload();
}

//append the html for the created branch
function createBranch(o) {
    jQuery('#Branches').parent().find('ul').prepend(jQuery.trim(o.Content));
    location.reload();
}

//append the html for the created product
function reloadView(o) {
   // jQuery('#Branches').parent().find('ul').prepend(jQuery.trim(o.Content));
    location.reload();
}

//append the html for the created country (inside the lookup popup)
function lookupCreateCountry(o) {
    jQuery('#CountryId-awepw .awe-srl').prepend(o.Content);
    jQuery('#CountryId-awepw .awe-srl .awe-li:first').hide().fadeIn();
}

function createPromotion(o) {
    jQuery('#Promotions').prepend(o.Content);
    jQuery('#Promotions li:first').hide().fadeIn();
    location.reload();
}

function passchanged(o) {
    jQuery("<div> password for " + o.Login + " was successfuly changed </div>").dialog();
}

function createUser(o) {
    jQuery('#Users').parent().find('tbody').prepend(o.Content);
    
}

function refreshBranchGrid() {
    jQuery('#BranchGrid').data('api').load();
}


function refreshFrequencyGrid() {
    jQuery('#FrequencyGrid').data('api').load();
}

function refreshLeadGrid() {
    jQuery('#LeadGrid').data('api').load();
}

function refreshMediaDocumentGrid() {
    jQuery('#MediaDocumentGrid').data('api').load();
}


function refreshTransactionGrid() {
    jQuery('#TransactionGrid').data('api').load();
}


function refreshPaymentGatewayGrid() {
    jQuery('#PaymentGatewayGrid').data('api').load();
}

function refreshIndustryGrid() {
    jQuery('#IndustryGrid').data('api').load();
}

function refreshAdGrid() {
    jQuery('#AdGrid').data('api').load();
}

function refreshConsumerGrid() {
    jQuery('#ConsumerGrid').data('api').load();
}

function refreshConsumerFeedbackGrid() {
    jQuery('#ConsumerFeedbackGrid').data('api').load();
}

function refreshProductCategoryGrid() {
    jQuery('#ProductCategoryGrid').data('api').load();
}

function refreshPromotionsGrid() {
    if (jQuery('#PromotionsGrid').length) {
        jQuery('#PromotionsGrid').data('api').load();
    }
}

function loadPromotionsGrid(r) {
    jQuery('#gridContainer').html(r);
    jQuery('.showGrid').hide();
}

// used for the grid to generate the delete button, will show a restore button if the item is deleted
function promotionsDelFormat(o) {
    var format = "<form class='" + (o.IsDeleted ? "PromotionRestore" : "PromotionConfirm") + "' method='post'>"
        + "<input type='hidden' name='Id' value='" + o.Id + "'/>"
        + "<button type='submit' class='awe-btn'>";
    
    if (o.IsDeleted) {
        format += "re";
    } else {
        format += "<span class='ico-del'></span>";
    }

    format += "</button>";
    format += "</form>";
    return format;
}

// adjusts the layout of the product items
function adjustProducts() {
    if (jQuery.support.cors)
        jQuery(".notcool").hide();
    else
        jQuery(".cool").hide();

    var w = jQuery('#main').width();
    var space = w % 492;
    var cat = (w - space) / 492;
    var u = (space / cat);
    var nw = 448 + u;
    jQuery('.product').width(nw);
    jQuery('.comments').css('width', jQuery('.comments:first').parent().width() - jQuery('.comments:first').prev().width() - 20);
}

jQuery(function () {

    jQuery("input:text:visible:first").focus();
    
    //bind the window min-height to window size
    jQuery(window).bind("resize", function (e) { jQuery("#main-container").css("min-height", (jQuery(window).height() - 120) + "px"); });
    
    jQuery("#main-container").css("min-height", (jQuery(window).height() - 120) + "px").addClass("ui-widget-content");

    //fix for IE to trigger change on enter
    if (!jQuery.support.cors) {
        jQuery("input[type=text]")
            .on("change", function (e) {
                jQuery.data(this, "value", this.value);
            })
            .on("keydown", function (e) {
                if (e.which === 13)
                    e.preventDefault();
            })
            .on("keyup", function (e) {
                e.preventDefault();
                if (e.which === 13 && this.value != jQuery.data(this, "value")) {
                    jQuery(this).change();
                }
            });
    }
});

function CKupdate() {
    for (instance in CKEDITOR.instances)
        CKEDITOR.instances[instance].updateElement();
    return true;
}

//http://www.w3schools.com/JS/js_cookies.asp
function setCookie(c_name, value, exdays) {
    var exdate = new Date();
    exdate.setDate(exdate.getDate() + exdays);
    var c_value = escape(value) + ((exdays == null) ? "" : "; expires=" + exdate.toUTCString());
    document.cookie = c_name + "=" + c_value;
}

function getCookie(c_name) {
    var i, x, y, ARRcookies = document.cookie.split(";");
    for (i = 0; i < ARRcookies.length; i++) {
        x = ARRcookies[i].substr(0, ARRcookies[i].indexOf("="));
        y = ARRcookies[i].substr(ARRcookies[i].indexOf("=") + 1);
        x = x.replace(/^\s+|\s+$/g, "");
        if (x == c_name) {
            return unescape(y);
        }
    }
}