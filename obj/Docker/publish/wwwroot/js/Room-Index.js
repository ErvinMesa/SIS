(function ($) {
    function RoomObj() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-Room").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new RoomObj();
        self.init();

        $('#modal-action-Room').on('show.bs.modal', function (event) {
            var button = $(event.relatedTarget); // Button that triggered the modal
            var url = button.attr("href");
            var modal = $(this);
            // note that this will replace the content of modal-content everytime the modal is opened
            modal.find('.modal-content').load(url);
        });

        $('#modal-action-Room').on('hidden.bs.modal', function () {
            // remove the bs.modal data attribute from it
            $(this).removeData('bs.modal');
            // and empty the modal-content element
            $('#modal-container .modal-content').empty();
        });


        $("#tblRoom").DataTable({
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
                "url": "/Room/LoadData",
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
                { "data": "RoomID", "name": "RoomID", "autoWidth": true, "visible": false },
                {
                    className: 'align-middle text-center',
                    "render": function (data, type, full, meta) { return '<a  id="tblBtnEdt"' + full.RoomID + ' class="btn btn-outline-info" data-toggle="modal" data-target="#modal-action-Room" href="/Room/AddEditRoom?RoomID=' + full.CollegeID + '"><i class="fa fa-pencil-alt"></i></a>'; }
                },
                {
                    className: 'align-middle text-center',
                    "render": function (data, type, full, meta) { return '<a  id="tblBtnDel"' + full.RoomID + ' class="btn btn-outline-danger" data-toggle="modal" data-target="#modal-action-Room" href="/Room/DeleteRoom?RoomID=' + full.RoomID + '"><i class="fa fa-trash"></i></a>'; }

                },
                { "data": "BuildingName", "name": "BuildingName", "autoWidth": false },
                { "data": "RoomCode", "name": "RoomCode", "autoWidth": false },
                { "data": "RoomName", "name": "RoomName", "autoWidth": false },
                { "data": "Capacity", "name": "Capacity", "autoWidth": false },
            ]

        });

        $("#createRoomModal").clone().appendTo("#tblRoom_filter");
    })


}(jQuery))