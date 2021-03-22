(function ($) {
    function StudentProfileObj() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-StudentProfile").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new StudentProfileObj();
        self.init();

        $('#modal-action-StudentProfile').on('show.bs.modal', function (event) {
            var button = $(event.relatedTarget); // Button that triggered the modal
            var url = button.attr("href");
            var modal = $(this);
            // note that this will replace the content of modal-content everytime the modal is opened
            modal.find('.modal-content').load(url);
        });

        $('#modal-action-StudentProfile').on('hidden.bs.modal', function () {
            // remove the bs.modal data attribute from it
            $(this).removeData('bs.modal');
            // and empty the modal-content element
            $('#modal-container .modal-content').empty();
        });


        $("#tblStudentProfile").DataTable({
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
                "url": "/StudentProfile/LoadData",
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
                { "data": "StudentID", "name": "StudentID", "autoWidth": true, "visible": false },
                {
                    className: 'align-middle text-center',
                    "render": function (data, type, full, meta) { return '<a  id="tblBtnEdt"' + full.StudentProfileID + ' class="btn btn-outline-info" data-toggle="modal" data-target="#modal-action-StudentProfile" href="/StudentProfile/AddEditStudentProfile?StudentID=' + full.StudentProfileID + '"><i class="fa fa-pencil-alt"></i></a>'; }
                },
                {
                    className: 'align-middle text-center',
                    "render": function (data, type, full, meta) { return '<a  id="tblBtnDel"' + full.StudentProfileID + ' class="btn btn-outline-danger" data-toggle="modal" data-target="#modal-action-StudentProfile" href="/StudentProfile/DeleteStudentProfile?StudentID=' + full.StudentProfileID + '"><i class="fa fa-trash"></i></a>'; }

                },
                { "data": "LastName", "name": "LastName", "autoWidth": false },
                { "data": "FirstName", "name": "FirstName", "autoWidth": false },
                { "data": "MiddleName", "name": "MiddleName", "autoWidth": false },
                { "data": "Gender", "name": "Gender", "autoWidth": false },
                { "data": "BirthDate", "name": "BirthDate", "autoWidth": false },
                { "data": "MobileNumber", "name": "MobileNumber", "autoWidth": false },
                { "data": "EmailAddress", "name": "EmailAddress", "autoWidth": false },
                { "data": "ProvinceName", "name": "ProvinceName", "autoWidth": false },
                { "data": "CityName", "name": "CityName", "autoWidth": false },
                { "data": "ProgramName", "name": "ProgramName", "autoWidth": false },
            ]

        });

        $("#createStudentProfileModal").clone().appendTo("#tblStudentProfile_filter");
    })


}(jQuery))