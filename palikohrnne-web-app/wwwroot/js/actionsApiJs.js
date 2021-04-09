let baseUrlApi = "http://localhost:8081";
let headers = {"content-type": "text/plain;charset=UTF-8"}

function likerCommentaire(idCitoyen, idCommentaire) {
    $.post({
        url: baseUrlApi + "/api/voteCommentaire",
        headers,
        data: JSON.stringify({
            CitoyenID: idCitoyen,
            CommentaireID: idCommentaire
        })
    })
}

function unlikerCommentaire(idCitoyen, idCommentaire) {
    $.ajax({
        method: "DELETE",
        url: baseUrlApi + "/api/voteCommentaire/" + idCitoyen + "/" + idCommentaire,
        headers
    })
}

function likerRessource(idCitoyen,idRessource) {
    $.post({
        url: baseUrlApi + "/api/voteRessources",
        headers,
        data: JSON.stringify({
            CitoyenID: idCitoyen,
            RessourceID: idRessource
        })
    })
}

function unlikerRessource(idCitoyen, idRessource) {
    $.ajax({
        method: "DELETE",
        url: baseUrlApi + "/api/voteRessources/" + idCitoyen + "/" + idRessource,
        headers
    })
}