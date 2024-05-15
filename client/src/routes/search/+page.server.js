

/** @type {import('./$types').Actions} */

import { Toast } from 'flowbite-svelte';
import { fromSaved, toSaved } from '../../stores/store';

export const actions = {

    /**
     * Retrieves weather information.
     * @typedef {import('$lib/weatherTypes').WeatherData} WeatherData
     * @param {object} params - Request parameters.
     * @param {Request} params.request - The request object.
     * @returns {Promise<{ success: boolean, from: WeatherData | null, to: WeatherData | null, line: import('$lib/weatherTypes').AirportResponse[] | null }>} A promise that resolves to an object containing success status and weather data.
     */
    get: async ({request}) => { 

        const data = await request.formData();
        const fromAirport = data.get('from');
        const toAirport = data.get('to');


        

        let response =  null;
        let fromJson = null;

        console.log("To:")

        console.log(toAirport)
        if(toAirport?.toString().length === 0 || toAirport === null){

            console.log("single airport")

        response = await fetch(`http://localhost:5249/api/Weather?ICAO=${fromAirport}`);
        fromJson = await response.json();
        console.log(fromJson)

        return { success: true, from: fromJson, to: null , line: null}
        }

        console.log("GetLine")

        const line = await fetch(`http://localhost:5249/byline?from=${fromAirport}&to=${toAirport}`)


        const lineJson = await line.json();


        console.log(lineJson)

        // let toWeatherInfo = null;

        // if(toAirport != null){
        //     // console.log(toAirport +  " Hej");

        //     const toResponse = await fetch(`http://localhost:5249/api/Weather?ICAO=${toAirport}`);
        //     const toJson = await toResponse.json();

        //     // /** @type {import('$lib/weatherTypes').WeatherData} */
        //     toWeatherInfo = toJson;

        //     // console.log(toWeatherInfo)



        // }

         


        // /** @type {import('$lib/weatherTypes').WeatherData} */


    return { success: true, from: null, to: null, line: lineJson}

}
};
