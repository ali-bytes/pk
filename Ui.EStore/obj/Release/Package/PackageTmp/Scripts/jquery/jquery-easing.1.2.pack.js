
//jQuery(function () {

//    jQuery('div.csw').prepend('<p class=\'loading\'>Loading...<br /><img src=\'images/ajax-loader.gif\' alt=\'loading...\'/ ></p>');
//});

//var j = 0;
//var count = 0;
//jQuery.fn.codaSlider = function (settings) {
//    settings = jQuery.extend({
//        easeFunc: 'expoinout',
//        easeTime: 750,
//        toolTip: false
//    }, settings);

//    function getCount(id) {
//        if (id >= count) {
//            id = 1;
//        } else if (id <= 0) {
//            id = count;
//        }

//        return id;

//    }


//    return this.each(function () {
//        var container = jQuery(this);
//        container.find('p.loading').remove();
//        container.removeClass('csw').addClass('stripViewer');
//        var panelWidth = container.find('div.panel').width();
//        var panelCount = container.find('div.panel').size();
//        var stripViewerWidth = panelWidth * panelCount;
//        container.find('div.panelContainer').css('width', stripViewerWidth);
//        var navWidth = panelCount * 2;
//        if (location.hash && parseInt(location.hash.slice(1)) <= panelCount) {
//            var cPanel = parseInt(location.hash.slice(1));
//            var cnt = -(panelWidth * (cPanel - 1));
//            jQuery(this).find('div.panelContainer').css({
//                left: cnt
//            });
//        }
//        else {
//            var cPanel = 1;
//        }

//        container.each(function (i) {

//            var toolId = parseInt(jQuery(this).attr('id'));
//            count++;
//            jQuery(this).before('<div class=\'stripNavL\' id=\'stripNavL' + j + '\'><a href=\'#' + 1 + '\'>Left</a></div>');
//            jQuery(this).after('<div class=\'stripNavR\' id=\'stripNavR' + j + '\'><a href=\'#' + 2 + '\'>Right</a></div>');

//            jQuery(this).before('<div class=\'stripNav\' id=\'stripNav' + j + '\' ><ul></ul></div>');
//            jQuery(this).find('div.panel').each(function (n) {
//                jQuery('div#stripNav' + j + ' ul').append('<li class=\'tab' + (n + 1) + '\'><a href=\'#' + jQuery(this).attr('id') + '\'>' + jQuery(this).attr('title') + '</a></li>');
             
//            });
//            jQuery('div#stripNav' + j + ' a').each(function (z) {
//                navWidth += jQuery(this).parent().width();
//                jQuery(this).bind('click', function () {
//                    jQuery(this).addClass('current').parent().parent().find('a').not(jQuery(this)).removeClass('current');
//                    var cnt = -(panelWidth * z);
//                    cPanel = z + 1;
//                    jQuery(this).parent().parent().parent().next().find('div.panelContainer').animate({
//                        left: cnt
//                    }, settings.easeTime, settings.easeFunc);
//                });
//            });
//            jQuery('div#stripNavL' + j + ' a').click(function () {
//                if (cPanel == 1) {
//                    var cnt = -(panelWidth * (panelCount - 1));
//                    cPanel = panelCount;
//                    jQuery(this).parent().parent().find('div.stripNav a.current').removeClass('current').parent().parent().find('li:last a').addClass('current');
//                } else {
//                    cPanel -= 1;
//                    var cnt = -(panelWidth * (cPanel - 1));
//                    jQuery(this).parent().parent().find('div.stripNav a.current').removeClass('current').parent().prev().find('a').addClass('current');
//                }
//                ;
//                jQuery(this).parent().parent().find('div.panelContainer').animate({
//                    left: cnt
//                }, settings.easeTime, settings.easeFunc);
//                location.hash = cPanel;
//                return false;
//            });
//            jQuery('div#stripNavR' + j + ' a').click(function () {
//                if (cPanel == panelCount) {
//                    var cnt = 0;
//                    cPanel = 1;
//                    jQuery(this).parent().parent().find('div.stripNav a.current').removeClass('current').parent().parent().find('a:eq(0)').addClass('current');
//                } else {
//                    var cnt = -(panelWidth * cPanel);
//                    cPanel += 1;
//                    jQuery(this).parent().parent().find('div.stripNav a.current').removeClass('current').parent().next().find('a').addClass('current');
//                }
//                ;
//                jQuery(this).parent().parent().find('div.panelContainer').animate({
//                    left: cnt
//                }, settings.easeTime, settings.easeFunc);
//                location.hash = cPanel;
//                return false;
//            });
//            jQuery('a.cross-link').click(function () {
//                jQuery(this).parents().find('.stripNav ul li a:eq(' + (parseInt(jQuery(this).slice(1)) - 1) + ')').trigger('click');
//            });
//            jQuery('div#stripNav' + j).css('width', navWidth);
//            if (location.hash && parseInt(location.hash.slice(1)) <= panelCount) {
//                jQuery('div#stripNav' + j + ' a:eq(' + (location.hash.slice(1) - 1) + ')').addClass('current');
//            } else {
//                jQuery('div#stripNav' + j + ' a:eq(0)').addClass('current');
//            }
//        });
//        j++;
//    });
   
//};
//jQuery(document).ready(function () {
//    jQuery('.current').click(function () {
//        jQuery('.stripViewer').fadeOut(3000);
   
//});
//});