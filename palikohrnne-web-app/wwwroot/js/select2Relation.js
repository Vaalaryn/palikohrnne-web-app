$("#select2-relation").select2({
    minimumInputLength: 3,
    ajax: {
        url: '/Relation/SearchCitoyen',
        data: function (params) {
            var query = {
                search: params.term,
                type: 'public'
            }

            // Query parameters will be ?search=[term]&type=public
            return query;
        }
    }
})