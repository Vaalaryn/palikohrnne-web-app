let timelines = [];

function _onBtnLikeClicked($button, saveInDb) {
    let id = parseInt($button.attr("data-id"));
    let $nbLikeDisplay = $button.parent().find(".nb-likes").first();
    let nbLikes = parseInt($nbLikeDisplay.text());


    if ($button.attr("data-type-like") == "ressource") {
        if (saveInDb) {
            likerRessource(2, id);
            updateNbLikes($nbLikeDisplay, nbLikes + 1);
        } else {
            unlikerRessource(2, id);
            updateNbLikes($nbLikeDisplay, nbLikes - 1);
        }
    } else {
        if (saveInDb) {
            likerCommentaire(2, id);
            updateNbLikes($nbLikeDisplay, nbLikes + 1);
            
        } else {
            unlikerCommentaire(2, id);
            updateNbLikes($nbLikeDisplay, nbLikes - 1);
        }
    }
}

function updateNbLikes($cible, nbToUpdate) {
    $cible.text(nbToUpdate);
}



$(document).ready(function () {
    $(".heart").each(function () {
        var scaleCurve = mojs.easing.path('M0,100 L25,99.9999983 C26.2328835,75.0708847 19.7847843,0 100,0');
        var el = $(this).get()[0];
        // mo.js timeline obj
        timeline = new mojs.Timeline();

        // tweens for the animation:

        // burst animation
        tween1 = new mojs.Burst({
            parent: el,
            radius: { 0: 100 },
            angle: { 0: 45 },
            y: -20,
            count: 10,
            radius: 55,
            children: {
                shape: 'circle',
                radius: 15,
                fill: ['red', 'blue'],
                strokeWidth: 100,
                duration: 500,
            }
        });


        tween2 = new mojs.Tween({
            duration: 900,
            onUpdate: function (progress) {
                var scaleProgress = scaleCurve(progress);
                el.style.WebkitTransform = el.style.transform = 'scale3d(' + scaleProgress + ',' + scaleProgress + ',1)';
            }
        });
        tween3 = new mojs.Burst({
            parent: el,
            radius: { 0: 100 },
            angle: { 0: -45 },
            y: -20,
            count: 10,
            radius: 40,
            children: {
                shape: 'circle',
                radius: 30,
                fill: ['white', 'red'],
                strokeWidth: 100,
                duration: 400,
            }
        });

        // add tweens to timeline:
        timeline.add(tween1, tween2, tween3);
        timelines.push(timeline);

        $(this).attr("data-index", timelines.length - 1);
    })



    //when clicking the button start the timeline/animation:
    $(".button.heart").click(function () {
        if ($(this).hasClass('active')) {
            $(this).removeClass('active');
            //Sauvegarde via l'api
            _onBtnLikeClicked($(this), false);
        } else {
            //Le commentaire a été liké
            $(this).addClass('active'); //On active le coeur
            timelines[parseInt($(this).attr("data-index"))].play(); // On lance l'animation

            //Sauvegarde via l'api
            _onBtnLikeClicked($(this), true);
        }
    });
});