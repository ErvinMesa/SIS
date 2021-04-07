(function ($) {
    function CourseObj() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-Course").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new CourseObj();
        self.init();

        $('#modal-action-Course').on('show.bs.modal', function (event) {
            var button = $(event.relatedTarget); // Button that triggered the modal
            var url = button.attr("href");
            var modal = $(this);
            // note that this will replace the content of modal-content everytime the modal is opened
            modal.find('.modal-content').load(url);
        });

        $('#modal-action-Course').on('hidden.bs.modal', function () {
            // remove the bs.modal data attribute from it
            $(this).removeData('bs.modal');
            // and empty the modal-content element
            $('#modal-container .modal-content').empty();
        });


        $("#tblCourse").DataTable({
            fixedColumns: true,
            scrollX: true,
            scrollY: true,
            scrollCollapse: true,
            paging: true,
            fixedColumns: {
                leftColumns: 3
            },
            responsive: true,
            "order": [[3, "asc"]],
            "processing": true, // for show progress bar
            "serverSide": true, // for process server side
            "filter": true, // this is for disable filter (search box)
            "orderMulti": false, // for disable multiple column at once
            "ajax": {
                "url": "/Course/LoadData",
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
                { "data": "CourseID", "name": "CourseID", "autoWidth": true, "visible": false },
                {
                    className: 'align-middle text-center',
                    "render": function (data, type, full, meta) { return '<a  id="tblBtnEdt"' + full.CourseID + ' class="btn btn-outline-info" data-toggle="modal" data-target="#modal-action-Course" href="/Course/AddEditCourse?CourseID=' + full.CourseID + '"><i class="fa fa-pencil-alt"></i></a>'; }
                },
                {
                    className: 'align-middle text-center',
                    "render": function (data, type, full, meta) { return '<a  id="tblBtnDel"' + full.CourseID + ' class="btn btn-outline-danger" data-toggle="modal" data-target="#modal-action-Course" href="/Course/DeleteCourse?CourseID=' + full.CourseID + '"><i class="fa fa-trash"></i></a>'; }

                },
                { "data": "CourseCode", "name": "CourseCode", "autoWidth": false },
                { "data": "CourseDescription", "name": "CourseDescription", "autoWidth": false },
                { "data": "AcademicUnit", "name": "AcademicUnit", "autoWidth": false },
                { "data": "ContactHours", "name": "ContactHours", "autoWidth": false },
                { "data": "TuitionUnit", "name": "TuitionUnit", "autoWidth": false },
            ]

        });

        $("#createCourseModal").clone().appendTo("#tblCourse_filter");
    })


}(jQuery))