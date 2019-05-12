$(() => {

    setInterval(() => {
        const like = GetLikeParameters(false)
        $.get('/home/canstilllike', like, function (csl) {
            if (!csl) {
                $('#like-btn').attr('disabled', true);
                $('#dislike-btn').attr('disabled', true);
            }
        })
    }, 100000);

    $('#like-btn').on('click', function () {
        const like = GetLikeParameters(true);

        $.post('/home/likedislikejoke', like, function () {
            $('#like-btn').attr('disabled', true);
            $('#dislike-btn').attr('disabled', false);
        });

    })

    $('#dislike-btn').on('click', function () {
        const like = GetLikeParameters(false)

        $.post('/home/likedislikejoke', like, function () {
            $('#dislike-btn').attr('disabled', true);
            $('#like-btn').attr('disabled', false);
        });

    })

    function GetLikeParameters(liked) {
        const like = {
            userId: $('#like-info').data('userid'),
            jokeId: $('#like-info').data('jokeid'),
            liked: liked
        }
        return like;
    }
})