let baseUrlApi = "http://localhost:44369";

function likerCommentaire(idCitoyen, idCommentaire) {
    $.post({
        url: "/Ressource/LikerCommentaire",
        data: {
            CitoyenID: idCitoyen,
            CommentaireID: idCommentaire
        }
    })
}

function unlikerCommentaire(idCitoyen, idCommentaire) {
    $.post({
        url: "/Ressource/UnlikerCommentaire",
        data: {
            CitoyenID: idCitoyen,
            CommentaireID: idCommentaire
        }
    })
}

function likerRessource(idCitoyen,idRessource) {
    $.post({
        url: "/Ressource/LikerRessource",
        data: {
            CitoyenID: idCitoyen,
            RessourceID: idRessource
        },

    })
}

function unlikerRessource(idCitoyen, idRessource) {
    $.post({
        url: "/Ressource/UnlikerRessource",
        data: {
            CitoyenID: idCitoyen,
            RessourceID: idRessource
        }
    })
}