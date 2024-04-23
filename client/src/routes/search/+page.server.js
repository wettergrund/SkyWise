

/** @type {import('./$types').Actions} */

export const actions = {

    /**
     * Retrieves weather information.
     * @typedef {import('$lib/weatherTypes').WeatherData} WeatherData
     * @param {object} params - Request parameters.
     * @param {Request} params.request - The request object.
     * @returns {Promise<{ success: boolean, from: WeatherData, to: WeatherData | null }>} A promise that resolves to an object containing success status and weather data.
     */
    get: async ({request}) => { 

        const data = await request.formData();
        const fromAirport = data.get('from');
        const toAirport = data.get('to');

        const response = await fetch(`http://localhost:5249/api/Weather?ICAO=${fromAirport}`);
        const json = await response.json();


        let toWeatherInfo = null;

        if(toAirport != null){
            console.log(toAirport +  " Hej");

            const toResponse = await fetch(`http://localhost:5249/api/Weather?ICAO=${toAirport}`);
            const toJson = await toResponse.json();

            // /** @type {import('$lib/weatherTypes').WeatherData} */
            toWeatherInfo = toJson;

            console.log(toWeatherInfo)



        }

         


        // /** @type {import('$lib/weatherTypes').WeatherData} */
        const weatherInfo = json;


    return { success: true, from: weatherInfo, to: toWeatherInfo }

}
};