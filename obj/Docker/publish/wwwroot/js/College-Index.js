(function ($) {
    function CollegeObj() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-College").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new CollegeObj();
        self.init();

        $('#modal-action-College').on('show.bs.modal', function (event) {
            var button = $(event.relatedTarget); // Button that triggered the modal
            var url = button.attr("href");
            var modal = $(this);
            // note that this will replace the content of modal-content everytime the modal is opened
            modal.find('.modal-content').load(url);
        });

        $('#modal-action-College').on('hidden.bs.modal', function () {
            // remove the bs.modal data attribute from it
            $(this).removeData('bs.modal');
            // and empty the modal-content element
            $('#modal-container .modal-content').empty();
        });


        $("#tblCollege").DataTable({
            fixedColumns: true,
            scrollX: true,
            scrollY: true,
            scrollCollapse: true,
            paging: true,
            fixedColumns: {
                leftColumns: 3
            },
            responsive: true,
            "order": [[4, "asc"]],
            "processing": true, // for show progress bar  
            "serverSide": true, // for process server side  
            "filter": true, // this is for disable filter (search box)  
            "orderMulti": false, // for disable multiple column at once  
            "ajax": {
                "url": "/College/LoadData",
                "type": "POST",
                "datatype": "json"
            },
            "columnDefs":
                [
                    {
                        "width": "5%", "targets": [1], "sortable": false

                    },
                    {
                        "width": "5%", "targets": [2], "sortable": false

                    },
                    {
                        "width": "5%", "targets": [3], "sortable": false

                    },
                    { "width": "10%", "targets": 3 }
                ],

            "columns": [
                { "data": "CollegeID", "name": "CollegeID", "autoWidth": true, "visible": false },
                {
                    className: 'align-middle text-center',
                    "render": function (data, type, full, meta) { return '<a  id="tblBtnUpl"' + full.CollegeID + ' class="btn btn-outline-danger" data-toggle="modal" data-target="#modal-action-College" href="/College/UploadLogo?CollegeID=' + full.CollegeID + '"><i class="fa fa-trash"></i></a>'; }

                },
                {
                    className: 'align-middle text-center',
                    "render": function (data, type, full, meta) { return '<a  id="tblBtnEdt"' + full.CollegeID + ' class="btn btn-outline-info" data-toggle="modal" data-target="#modal-action-College" href="/College/AddEditCollege?CollegeID=' + full.CollegeID + '"><i class="fa fa-pencil-alt"></i></a>'; }
                },
                {
                    className: 'align-middle text-center',
                    "render": function (data, type, full, meta) { return '<a  id="tblBtnDel"' + full.CollegeID + ' class="btn btn-outline-danger" data-toggle="modal" data-target="#modal-action-College" href="/College/DeleteCollege?CollegeID=' + full.CollegeID + '"><i class="fa fa-trash"></i></a>'; }

                },
                { "data": "CollegeCode", "name": "CollegeCode", "autoWidth": false },
                { "data": "CollegeName", "name": "CollegeName", "autoWidth": false },
                { "data": "NameofDean", "name": "NameofDean", "autoWidth": false }
            ]

        });

        $("#createCollegeModal").clone().appendTo("#tblCollege_filter");
    })


}(jQuery))   