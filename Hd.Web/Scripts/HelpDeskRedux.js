
$(document).ready(function () {
	// bind your jQuery events here initially
	allFunctions();
});

var prm = Sys.WebForms.PageRequestManager.getInstance();


prm.add_endRequest(function () {
	// re-bind your jQuery events here
	allFunctions();
});

prm.add_beginRequest(function () {
	$(".waitDialog").center();
});

function allFunctions() {
	function doWait() {
		$(".waitDialog").center();
	}

	// hide the waitDialog if already visible
	$(".waitDialog").css("display", "none");


	$("#aspnetForm").on('onsubmit', function (e) {
		$(".waitDialog").center();
	});

	// If something is invalid, do NOT show the waitDialog
	$('form').submit(function () {
		if (typeof Page_Validators != 'undefined') {
			$.each(Page_Validators, function () {
				if (!this.isvalid) {
					$(".waitDialog").css("display", "none");
				}
			});
		}
	});
	jQuery.fn.center = function () {
		this.css("display", "block")
		this.css("position", "absolute");
		this.css("top", Math.max(0, (($(window).height() - $(this).outerHeight()) / 2) + $(window).scrollTop()) + "px");
		this.css("left", Math.max(0, (($(window).width() - $(this).outerWidth()) / 2) + $(window).scrollLeft()) + "px");
		return this;
	}


	// Activate alert div
	$(".alert").alert();
	// Hide alert
	if ($("#ctl00_lblLastAction").length == 0) {
		$(".alert").alert("close");
		$("#ctl00_pnlLastAction").css({display: "none"});
	}


	// Fix copyright notice to bottom if the page does not fill the height of the window
	if ($(window).height() > ($(document).height() - 200)) {
		$("#copy").css({
			position: "absolute",
		});
	}
	// hide popup frame
	$("#popper").hide();
	function adjustBar() {
		if (window == window.top) {
			// the page is at the topmost level, and not inside an iframe
			$(".darkbar2").css({ height: $(window).innerHeight() + "px", width: $(".darkbar").width() + "px" });
		}
		else {
			// the page is inside an iframe
			// this allows us to open the same link in both iframe (with excess parts hidden)
			// or open in new tab/window and see the header + nav
			$("#top").hide();
			$(".darkbar").hide();
			$("#darkbar2").hide();
		}
	}
	adjustBar();
	$(window).on('resize', function () {
		adjustBar();
	});
	$(".requestName").on('click', function (e) {
		// navigate to iframe, and show
		e.preventDefault();
		var href = $(this).attr("href");
		loadIframe("popframe", href);
		$("#popper").show();
	});

	$(".voter").on('click', function (e) {
		doWait();
		$(this).addClass("disabled");
	});
	$(".tab").on('click', function (e) {
		doWait();
		var anchor = $(this).find("a");
		window.location = anchor.attr("href");
	});
	$(".clearTab").on('click', function (e) {
		doWait();
		var anchor = $(this).find("a");
		window.location = anchor.attr("href");
	});
	$("#popclose").on('click', function (e) {
		closeIframe();
	});
	$("#popper").on('click', function (e) {
		closeIframe();
	});
	function closeIframe() {
		$("#popper").hide();
		loadIframe("popframe", "about:blank");
	}
	// force link 'click' when the row is clicked
	$(".itemRow").on('click', function (e) {
		var $thisCell, $tgt = $(e.target);
		if ($tgt.is('.editLink'))
			return;
		var anchor = $(this).find(".requestName");
		loadIframe("popframe", anchor.attr("href"));
		anchor.click();
	});
	// function to load a url into an iframe
	function loadIframe(iframeName, url) {
		var $iframe = $('#' + iframeName);
		if ($iframe.length) {
			$iframe.attr('src', url);
			return false;
		}
		return true;
	}
}