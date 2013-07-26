
ImmapMenuHandle = function () {
    // Get Url
    var pathname = window.location.pathname;
    // Store variables
    var accordion_head = $('.accordion > li > a'),
			accordion_body = $('.accordion li > .sub-menu');
    if ((pathname.match(/User$/)) || (pathname.match(/User.aspx$/))) {
        $('a[href$="#General"]').addClass('active').next().slideDown('normal');
        $('a[href$="User"]').addClass('highlight');
    }
    else if ((pathname.match(/UserGroup$/)) || (pathname.match(/UserGroups.aspx$/))) {
        $('a[href$="#General"]').addClass('active').next().slideDown('normal');
        $('a[href$="UserGroup"]').addClass('highlight');
    }
    else if ((pathname.match(/General$/)) || (pathname.match(/GeneralSettings.aspx$/))) {
        $('a[href$="#General"]').addClass('active').next().slideDown('normal');
        $('a[href$="General"]').addClass('highlight');
    }
    else if ((pathname.match(/Map$/)) || (pathname.match(/MapViewSettings.aspx$/))) {
        $('a[href="#Map"]').addClass('active').next().slideDown('normal');
        $('a[href$="Map"]').addClass('highlight');
    }
    else if ((pathname.match(/WebTiles$/)) || (pathname.match(/WebTiles.aspx$/))) {
        $('a[href="#Map"]').addClass('active').next().slideDown('normal');
        $('a[href$="WebTiles"]').addClass('highlight');
    }
    else if ((pathname.match(/MapLibrary$/)) || (pathname.match(/TtkGISProjectDef.aspx$/))) {
        $('a[href="#Map"]').addClass('active').next().slideDown('normal');
        $('a[href$="MapLibrary"]').addClass('highlight');
    }
    else if ((pathname.match(/DataViews$/)) || (pathname.match(/GISGridTableSettings.aspx$/))) {
        $('a[href="#Map"]').addClass('active').next().slideDown('normal');
        $('a[href$="DataViews"]').addClass('highlight');
    }
    else if ((pathname.match(/Themes$/)) || (pathname.match(/Themes.aspx$/))) {
        $('a[href="#Map"]').addClass('active').next().slideDown('normal');
        $('a[href$="Themes"]').addClass('highlight');
    }
    else if ((pathname.match(/ThemeGroups$/)) || (pathname.match(/ThemeGroups.aspx$/))) {
        $('a[href="#Map"]').addClass('active').next().slideDown('normal');
        $('a[href$="ThemeGroups"]').addClass('highlight');
    }
    else if ((pathname.match(/GeoBookMarksCategories$/)) || (pathname.match(/GeoBookMarksCategories.aspx$/))) {
        $('a[href="#Map"]').addClass('active').next().slideDown('normal');
        $('a[href$="GeoBookMarksCategories"]').addClass('highlight');
    }
    else if ((pathname.match(/GeoBookMarks$/)) || (pathname.match(/GeoBookMarks.aspx$/))) {
        $('a[href="#Map"]').addClass('active').next().slideDown('normal');
        $('a[href$="GeoBookMarks"]').addClass('highlight');
    }
    else if ((pathname.match(/DynamicModules$/)) || (pathname.match(/DynamicDataDefsNew.aspx$/))) {
        $('a[href="#Modules"]').addClass('active').next().slideDown('normal');
        $('a[href$="DynamicModules"]').addClass('highlight');
    }
    else if ((pathname.match(/RSS$/)) || (pathname.match(/Feeds.aspx$/))) {
        $('a[href="#Modules"]').addClass('active').next().slideDown('normal');
        $('a[href$="RSS"]').addClass('highlight');
    }
    else if ((pathname.match(/RSSGroups$/)) || (pathname.match(/FeedGroups.aspx$/))) {
        $('a[href="#Modules"]').addClass('active').next().slideDown('normal');
        $('a[href$="RSSGroups"]').addClass('highlight');
    }
    else if ((pathname.match(/Security$/)) || (pathname.match(/SecurityModuleSettings.aspx$/))) {
        $('a[href="#Modules"]').addClass('active').next().slideDown('normal');
        $('a[href$="Security"]').addClass('highlight');
    }
    else if ((pathname.match(/Formatting$/)) || (pathname.match(/DynamicDataManager.aspx$/))) {
        $('a[href="#Dynamic Settings"]').addClass('active').next().slideDown('normal');
        $('a[href$="Formatting"]').addClass('highlight');
    }
    else if ((pathname.match(/Validation$/)) || (pathname.match(/Validation.aspx$/))) {
        $('a[href="#Dynamic Settings"]').addClass('active').next().slideDown('normal');
        $('a[href$="Validation.aspx"]').addClass('highlight');
    }
    else if ((pathname.match(/NearbyFeatures$/)) || (pathname.match(/NearbyFeatures.aspx$/))) {
        $('a[href="#Dynamic Settings"]').addClass('active').next().slideDown('normal');
        $('a[href$="NearbyFeatures.aspx"]').addClass('highlight');
    }
    else if ((pathname.match(/Charts$/)) || (pathname.match(/ChartSettings.aspx$/))) {
        $('a[href="#Dynamic Settings"]').addClass('active').next().slideDown('normal');
        $('a[href$="ChartSettings"]').addClass('highlight');
    }
    else if ((pathname.match(/Queries$/)) || (pathname.match(/Queries.aspx$/))) {
        $('a[href="#Dynamic Settings"]').addClass('active').next().slideDown('normal');
        $('a[href$="Queries.aspx"]').addClass('highlight');
    }

    // Click function
    accordion_head.on('click', function (event) {
        // Disable header links
        event.preventDefault();
        // Show and hide the tabs on click
        if ($(this).attr('class') != 'active') {
            accordion_body.slideUp('normal');
            $(this).next().stop(true, true).slideToggle('normal');
            accordion_head.removeClass('active');
            $(this).addClass('active');
        }
    });
}