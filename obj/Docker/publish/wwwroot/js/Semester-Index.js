(function ($) {
    function SemesterObj() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-Semester").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new SemesterObj();
        self.init();

        $('#modal-action-Semester').on('show.bs.modal', function (event) {
            var button = $(event.relatedTarget); // Button that triggered the modal
            var url = button.attr("href");
            var modal = $(this);
            // note that this will replace the content of modal-content everytime the modal is opened
            modal.find('.modal-content').load(url);
        });

        $('#modal-action-Semester').on('hidden.bs.modal', function () {
            // remove the bs.modal data attribute from it
            $(this).removeData('bs.modal');
            // and empty the modal-content element
            $('#modal-container .modal-content').empty();
        });


        $("#tblSemester").DataTable({
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
                "url": "/Semester/LoadData",
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
                    { "width": "10%", "targets": 3 }
                ],

            "columns": [
                { "data": "AYSemID", "name": "SemesterID", "autoWidth": true, "visible": false },
                {
                    className: 'align-middle text-center',
                    "render": function (data, type, full, meta) { return '<a  id="tblBtnEdt"' + full.SemesterID + ' class="btn btn-outline-info" data-toggle="modal" data-target="#modal-action-Semester" href="/Semester/AddEditSemester?SemesterID=' + full.SemesterID + '"><i class="fa fa-pencil-alt"></i></a>'; }
                },
                {
                    className: 'align-middle text-center',
                    "render": function (data, type, full, meta) { return '<a  id="tblBtnDel"' + full.SemesterID + ' class="btn btn-outline-danger" data-toggle="modal" data-target="#modal-action-Semester" href="/Semester/DeleteSemester?SemesterID=' + full.SemesterID + '"><i class="fa fa-trash"></i></a>'; }

                },
                { "data": "AcademicYear", "name": "AcademicYear", "autoWidth": false },
                { "data": "SemesterName", "name": "SemesterName", "autoWidth": false },
            ]

        });

        $("#createSemesterModal").clone().appendTo("#tblSemester_filter");
    })


}(jQuery))