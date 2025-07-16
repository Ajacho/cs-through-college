document.addEventListener("DOMContentLoaded", function () {
	const searchBtn = document.getElementById("searchMovieButton");
	searchBtn.addEventListener("click", searchMovie);
});


async function searchMovie() {
	const searchInput = document.getElementById("searchMovieInput").value;

	if (searchInput === "") {
		alert("Please enter a movie name");
		return;
	}

	const response = await fetch(`/api/APIMovieSearch/search/movie?query=${searchInput}`);
	const searchResults = await response.text();
	if (response.ok) {
		const movies = JSON.parse(searchResults);
		if (movies.length === 0) {
			alert("No movies found for the given query");
			return;
		}
		console.log(movies);
		displayMovies(movies);
	} else {
		alert("Error fetching: " + searchResults);
	}

}

function displayMovies(movies) {

	movies.sort((a, b) => b.popularity - a.popularity);

    const movieList = document.getElementById("movieList");
    movieList.innerHTML = "";

		movies.forEach(movie => {
			const movieElement = document.createElement("div");
			movieElement.classList.add("col-md-12", "mb-4");
			movieElement.innerHTML = `
				<div class="card mb-3" data-movie-id="${movie.id}">
					<div class="row g-0">
						<div class="col-md-4" >
							<img src="https://image.tmdb.org/t/p/w500${movie.poster_path}" class="img-fluid rounded-start posterImg" alt="...">
						</div>
						<div class="col-md-8">
							<div class="card-body">
								<h5 class="card-title">${movie.title}</h5>
								<p class="card-text">${movie.release_date}</p>
								<p class="card-text overview">${movie.overview}</p>
								<p class="card-link"><a href="https://www.themoviedb.org/movie/${movie.id}" target="_blank">Watch Trailer here :D</a></p>
							</div>
						</div>
					</div>
				</div>
				`;
			movieElement.addEventListener("click", () => displayModalMovieDetails(movie.id));
			movieList.appendChild(movieElement);
		});

}


async function displayModalMovieDetails(movieId) {
	const modal = new bootstrap.Modal(document.getElementById("myModal"));
	modal.show();

	try {
		const [movieResponse, creditsResponse] = await Promise.all([
			fetch(`/api/APIMovieSearch/movie/${movieId}`),
			fetch(`/api/APIMovieSearch/movie/${movieId}/credits`)
		]);

		if (!movieResponse.ok) {
			throw new Error(`HTTP error! status: ${movieResponse.status}`);
		}
		if (!creditsResponse.ok) {
			throw new Error(`HTTP error! status: ${creditsResponse.status}`);
		}

		const movie = await movieResponse.json();
		const credits = await creditsResponse.json();

		console.log("Note for myself: Clear Catche :D");
		console.log(credits);

		// Format runtime in hours and minutes
		const hours = Math.floor(movie.runtime / 60);
		const minutes = movie.runtime % 60;
		movie.runtime = `${hours}h ${minutes}m`;

		// Revenue in USD
		movie.revenue = `$${movie.revenue.toLocaleString()}`;

		// Getting just year from release date
		const year = movie.release_date.split("-")[0];

		// Getting genres
		movie.genres = movie.genres.map(genre => genre.name).join(", ");

		// Month date year format
		const dateFormat = new Date(movie.release_date).toLocaleDateString('en-US', {
			year: 'numeric',
			month: 'long',
			day: 'numeric'
		});
		movie.dateFormat = dateFormat;


		const modalContent = document.getElementById("movieDetails");
		modalContent.innerHTML = `
				<div class="modal-header">
					<h5 class="modal-title" id="exampleModalLabel">${movie.title} (${year})</h5>
					<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
				</div>
				<div class="modal-body">
					<div class="row g-0">
						<div class="col-md-4">
							<img src="https://image.tmdb.org/t/p/w500${movie.backdrop_path}" class="img-fluid rounded-start posterImg" alt="Movie poster">
						</div>
						<div class="col-md-8">
							<div class="card-body">
								<p class="card-text"> Released on: ${movie.release_date}</p>
								<p class="card-text">Genres: ${movie.genres}</p>
								<p class="card-text">${movie.overview}</p>
								<p class="card-text">${movie.runtime}</p>
								<p class="card-text">Polularity: ${movie.popularity}</p>
								<p class="card-text">Revenue: ${movie.revenue}</p>
								<p class="card-link"><a href="https://www.themoviedb.org/movie/${movie.id}" target="_blank">Watch Trailer here :D</a></p>
							</div>
						</div>
					</div>
					<div class="row g-0">
                        <div class="col-md-12">
                            <h5>Cast</h5>
                            ${credits.cast ? credits.cast.slice(0, 10).map(cast => `<li class="list-group-item">${cast.original_name} as "${cast.character}"</li>`).join("") : "No cast information available"}
											
						</div>
                    </div>
				</div>
			`;
	} catch (error) {
		console.error("Failed to fecth movie details", error);
		alert("Failed to fetch movie/credits details");
	}

}

