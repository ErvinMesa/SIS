(function ($) {
    function ProgramObj() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-Program").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new ProgramObj();
        self.init();

        $('#modal-action-Program').on('show.bs.modal', function (event) {
            var button = $(event.relatedTarget); // Button that triggered the modal
            var url = button.attr("href");
            var modal = $(this);
            // note that this will replace the content of modal-content everytime the modal is opened
            modal.find('.modal-content').load(url);
        });

        $('#modal-action-Program').on('hidden.bs.modal', function () {
            // remove the bs.modal data attribute from it
            $(this).removeData('bs.modal');
            // and empty the modal-content element
            $('#modal-container .modal-content').empty();
        });


        var tblDiv = $("#tblProgram").DataTable({

            fixedColumns: false,
            scrollX: true,
            scrollY: 300,
            scrollCollapse: true,
            paging: true,
            responsive: false,
            processing: true, // for show progress bar  
            serverSide: true, // for process server side  
            filter: true, // this is for disable filter (search box)  
            orderMulti: false, // for disable multiple column at once  
            "order": [[0, "desc"]],
            "ajax": {
                "url": "/EnrolledProgram/LoadData",
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

                    }],

            "columns": [
                { "data": "ProgramID", "name": "ProgramID", "autoWidth": true, "visible": false },
                {
                    className: 'align-middle text-center',
                    "render": function (data, type, full, meta) { return '<a  id="tblBtnEdt"' + full.ProgramID + ' class="btn btn-outline-info" href="/ProgramMaster/Edit?ProgramID=' + full.ProgramID + '"><i class="fa fa-pencil-alt"></i></a>'; }
                },
                {
                    className: 'align-middle text-center',
                    "render": function (data, type, full, meta) { return '<a  id="tblBtnDel"' + full.ProgramID + ' class="btn btn-outline-danger" data-toggle="modal" data-target="#modal-action-Program" href="/ProgramMaster/DeleteProgram?ProgramID=' + full.ProgramID + '"><i class="fa fa-trash"></i></a>'; }

                },
                { "data": "ProgramCode", "name": "ProgramCode", "autoWidth": true },
                { "data": "ProgramName", "name": "ProgramName", "autoWidth": true },
                { "data": "CollegeName", "name": "CollegeName", "autoWidth": true },
                { "data": "NumberofSemester", "name": "NumberofSemester", "autoWidth": true }
            ]

        });


        $("#createProgramModal").clone().appendTo("#tblProgram_filter");
    })
}(jQuery))   