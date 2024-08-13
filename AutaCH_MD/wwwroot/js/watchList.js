function initializeWatchList() {
    const watchList = JSON.parse(localStorage.getItem('watchlist')) || {};

    document.querySelectorAll('[data-car-id]').forEach(button => {
        const carId = button.getAttribute('data-car-id');
        const icon = button.querySelector('i');

        if (watchList[carId]) {
            icon.style.color = "red";
        } else {
            icon.style.color = "white";
        }

        button.addEventListener('click', function () {
            if (watchList[carId]) {
                delete watchList[carId];
                icon.style.color = "white";
            } else {
                watchList[carId] = true;
                icon.style.color = "red";
            }

            localStorage.setItem('watchlist', JSON.stringify(watchList));
            sendWatchListToServer();
        });
    });
    
}

function sendWatchListToServer() {
    const watchList = JSON.parse(localStorage.getItem('watchlist')) || {};
    const carIds = Object.keys(watchList);

    if (carIds.length === 0) {
        return;
    }

    fetch("Account/GetWatchList", {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(carIds)
    })
        .then(response => response.text())
        .then(html => { 
            document.querySelector('.car-cards').innerHTML = html;
        })
        .catch(error => {
            console.error("Error fetching watch list: ", error);
        })

}




