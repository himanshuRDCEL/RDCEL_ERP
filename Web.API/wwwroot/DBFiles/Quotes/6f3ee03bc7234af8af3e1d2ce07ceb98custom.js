$(document).ready( function () {
    var  DT1 = $('#leadtbl').DataTable( {
        info: false,
        scrollX: true,
        searching: false,
        sDom: 'tl',
        columnDefs: [{
            orderable: false,
            className: 'no-sort',
            targets: [-1, 0]
        }],
    } );
 
  } );

  $(document).ready( function () {
    var  DT1 = $('#notetbl').DataTable( {
        info: false,
        scrollX: true,
        searching: false,
        paging: false,
        sDom: 'tl',
        columnDefs: [{
            orderable: false,
            className: 'no-sort',
            targets: [-1, 0]
        }],
    } );
  } );

  $(document).ready( function () {
    var  DT1 = $('#attachmenttbl').DataTable( {
        info: false,
        scrollX: true,
        searching: false,
        paging: false,
        sDom: 'tl',
        columnDefs: [{
            orderable: false,
            className: 'no-sort',
            targets: [-1, 0]
        }],
    } );
  } );


$('#dashtbl').DataTable( {
    searching: false, paging: false, info: false
} ); 


$('#activitytbl').DataTable( {
    info: false,
    scrollX: true,
    searching: false
} );


$('.treeview').on('click', function (event) {
    $target = $(event.target);   
       $target.toggleClass('treeview-show');
});

$('.treeviewicon').on('click', function (event) {
    $target = $(event.target);   
       $target.toggleClass('fa-angle-right fa-angle-down');
});

$(".treeview a").click(function() { 
    $(".treeview a").not($(this)).removeClass('treeview-show');
});




$(".actionbtn").hide();

$("#leadtbl input").click(function() {
    if($(this).is(":checked")) {
        $("#maintbl .actionbtn").show(300);
    }else
    $("#maintbl .actionbtn").hide(300);
});

$("#attachmenttbl input").click(function() {
    if($(this).is(":checked")) {
        $("#attachmentwidget .actionbtn").show(300);
    }else
    $("#attachmentwidget .actionbtn").hide(300);
});

$("#notetbl input").click(function() {
    if($(this).is(":checked")) {
        $("#notewidget .actionbtn").show(300);
    }else
    $("#notewidget .actionbtn").hide(300);
});

$(".selectAll").click(function(){
    $('input:checkbox').not(this).prop('checked', this.checked);
});

$(".selectAllNotes").click(function(){
    $('#notetbl input:checkbox').not(this).prop('checked', this.checked);
});

$(".selectAllAttachment").click(function(){
    $('#attachmenttbl input:checkbox').not(this).prop('checked', this.checked);
});

$.fn.extend({
    treed: function (o) {
      
      var openedClass = 'fa-minus-square';
      var closedClass = 'fa-plus-square';
      
      if (typeof o != 'undefined'){
        if (typeof o.openedClass != 'undefined'){
        openedClass = o.openedClass;
        }
        if (typeof o.closedClass != 'undefined'){
        closedClass = o.closedClass;
        }
      };
      
        //initialize each of the top levels
        var tree = $(this);
        tree.addClass("tree");
        tree.find('li').has("ul").each(function () {
            var branch = $(this); //li with children ul
            branch.prepend("<i class='indicator far " + closedClass + "'></i>");
            branch.addClass('branch');
            branch.on('click', function (e) {
                if (this == e.target) {
                    var icon = $(this).children('i:first');
                    icon.toggleClass(openedClass + " " + closedClass);
                    $(this).children().children().toggle();
                }
            })
        });
        //fire event from the dynamically added icon
      tree.find('.branch .indicator').each(function(){
        $(this).on('click', function () {
            $(this).closest('li').click();
        });
      });

        //fire event to open branch if the li contains a button instead of text
        tree.find('.branch>button').each(function () {
            $(this).on('click', function (e) {
                $(this).closest('li').click();
                e.preventDefault();
            });
        });
    }
});



$('#tree3').treed({openedClass:'fa-plus-square', closedClass:'fa-minus-square'});

$("#togglebtn").click(function(){
    $("#mainsidebar-wrapper").toggleClass("col-md-3");
    $("#mainsidebar-wrapper").toggleClass("col-md-2");
    $("#mainsidebar").toggleClass("d-none");
    $("#mainsidebar").toggleClass("col-md-4");
    $("#mainsidebar").toggleClass("minsidebar");
    $("#togglesidebar").toggleClass("col-md-8");
    $("#togglesidebar").toggleClass("col-md-12");
    $("#main").toggleClass("col-md-9");
    $("#main").toggleClass("col-md-10 main-noleft");
    $("#toggleicon").toggleClass("fa-chevron-left");
    $("#toggleicon").toggleClass("fa-chevron-right");
    $("#togglebtn").toggleClass("hiddentoggle");
    $("#togglesidebarhidden").toggleClass("col-md-12");
  });

  $("#topsidebartoggle").click(function(){
    $("#mainsidebar-wrapper").toggleClass("col-md-3");
    $("#mainsidebar-wrapper").toggleClass("col-md-3");
    $("#mainsidebar ul").toggleClass("d-none");
    $("#mainsidebar").toggleClass("minsidebar");
    $("#togglesidebar").toggleClass("col-md-8");
    $("#togglesidebar").toggleClass("col-md-12");
    $("#main").toggleClass("col-md-9");
    $("#main").toggleClass("col-md-10");
    $("#toggleicon").toggleClass("fa-chevron-up");
    $("#toggleicon").toggleClass("fa-chevron-down");
    $("#togglebtn").toggleClass("");
    $("#togglesidebarhidden").toggleClass("col-md-12");
  });

  $("#navtoggle").click(function () {
    $("#subnav ul").toggle();
});

var ctx = document.getElementById('monthlyleads');
var myChart = new Chart(ctx, {
    type: 'line',
    data: {
        labels: ['May', 'June', 'July', 'Aug', 'Sep', 'Oct'],
        datasets: [{
            label: 'Leads Each Month',
            data: [120, 190, 130, 200, 190, 260],
            backgroundColor: [
                'rgba(255, 99, 132)',
                'rgba(54, 162, 235)',
                'rgba(255, 206, 86)',
                'rgba(75, 192, 192)',
                'rgba(153, 102, 255)',
                'rgba(255, 159, 64)'
            ],
            borderColor: [
                'rgba(255, 99, 132, 1)',
                'rgba(54, 162, 235, 1)',
                'rgba(255, 206, 86, 1)',
                'rgba(75, 192, 192, 1)',
                'rgba(153, 102, 255, 1)',
                'rgba(255, 159, 64, 1)'
            ],
            borderWidth: 1
        }]
    },
    options: {
        scales: {
            y: {
                beginAtZero: true
            }
        }
    }
});

var topsource = document.getElementById('topsource');
var chart2 = new Chart(topsource, {
    type: 'pie',
    data: {
        labels: ['Website', 'Social Media', 'Paid Advertising', 'Direct'],
        datasets: [{
            label: 'Leads Each Month',
            data: [23,46,67,34],
            backgroundColor: [
                'rgba(255, 99, 132)',
                'rgba(54, 162, 235)',
                'rgba(153, 102, 255)',
                'rgba(255, 159, 64)',
            ],
       
            borderWidth: 1
        }]
    },
    options: {
        scales: {
            y: {
                beginAtZero: true
            }
        }
    }
});


$(this).prop('checked', false);

$(document).ready(function(){
$('#togglesidebarhidden').hide();
$("#salesnav").click(function(){
    $("#togglesidebarhidden").toggle();
    $("#main").toggleClass("col-md-11 hiddensidebarmain col-md-9");
});
});