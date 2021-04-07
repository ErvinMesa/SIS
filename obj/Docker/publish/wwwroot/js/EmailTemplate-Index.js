(function ($) {
    function EmailTemplateObj() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-EmailTemplate").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new EmailTemplateObj();
        self.init();

        $('#modal-action-EmailTemplate').on('show.bs.modal', function (event) {
            var button = $(event.relatedTarget); // Button that triggered the modal
            var url = button.attr("href");
            var modal = $(this);
            // note that this will replace the content of modal-content everytime the modal is opened
            modal.find('.modal-content').load(url);
        });

        $('#modal-action-EmailTemplate').on('hidden.bs.modal', function () {
            // remove the bs.modal data attribute from it
            $(this).removeData('bs.modal');
            // and empty the modal-content element
            $('#modal-container .modal-content').empty();
        });


        var tblDiv = $("#tblEmailTemplate").DataTable({

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
            "order": [[3, "asc"]],
            "ajax": {
                "url": "/EmailTemplate/LoadData",
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
                { "data": "TemplateID", "name": "TemplateID", "autoWidth": true, "visible": false },
                {
                    className: 'align-middle text-center',
                    "render": function (data, type, full, meta) { return '<a  id="tblBtnEdt"' + full.TemplateID + ' class="btn btn-outline-info" href="/EmailTemplate/Edit?TemplateID=' + full.TemplateID + '"><i class="fa fa-pencil-alt"></i></a>'; }
                },
                {
                    className: 'align-middle text-center',
                    "render": function (data, type, full, meta) { return '<a  id="tblBtnDel"' + full.TemplateID + ' class="btn btn-outline-danger" data-toggle="modal" data-target="#modal-action-EmailTemplate" href="/EmailTemplate/DeleteEmailTemplate?TemplateID=' + full.TemplateID + '"><i class="fa fa-trash"></i></a>'; }

                },
                { "data": "TemplateName", "name": "TemplateName", "autoWidth": true },
                { "data": "EmailTo", "name": "EmailTo", "autoWidth": true },
                { "data": "EmailCC", "name": "EmailCC", "autoWidth": true },
                { "data": "SubjectContent", "name": "SubjectContent", "autoWidth": true },
            ]

        });


        $("#createEmailTemplateModal").clone().appendTo("#tblEmailTemplate_filter");
    })
}(jQuery))   