let baseUrlApi = "http://localhost:8081";
let headers = {"content-type": "text/plain;charset=UTF-8"}

function likerCommentaire(idCitoyen, idCommentaire) {
    $.post({
        url: baseUrlApi + "/voteCommentaire",
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
        url: baseUrlApi + "/voteCommentaire/" + idCitoyen + "/" + idCommentaire,
        headers
    })
}

function likerRessource(idCitoyen,idRessource) {
    $.post({
        url: baseUrlApi + "/voteRessources",
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
        url: baseUrlApi + "/voteRessources/" + idCitoyen + "/" + idRessource,
        headers
    })
}