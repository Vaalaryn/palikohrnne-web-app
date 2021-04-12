



$(() => {
    //Init simditor
    var editor = new Simditor({
        textarea: $('#Contenu')
        //optional options
    });

    $("#Tags").select2({
        tags: true
    });
})