$("#select2Tags").select2();
$("#showFilter").click(() => {
    if ($(".filtres-container:not(:visible)").length > 0) {
        $(".filtres-container:not(:visible)").slideDown("slow");
    }
    else {
        $(".filtres-container:visible").slideUp("slow");
    }
})