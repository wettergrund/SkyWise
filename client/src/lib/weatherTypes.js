/**
 * @typedef {Object} CloudLayer
 * @property {string} cover - The cloud cover type.
 * @property {number} cloudBase - The altitude of the cloud base.
 * @property {string} cloudType - The type of cloud.
 */

/**
 * @typedef {Object} MetarInfo
 * @property {number} id - The ID of the METAR information.
 * @property {string} rawMetar - The raw METAR data.
 * @property {string} icao - The ICAO code.
 * @property {string} validFrom - The valid start time of the METAR.
 * @property {number} temp - The temperature.
 * @property {number} dewPoint - The dew point.
 * @property {number} windDirectionDeg - The wind direction in degrees.
 * @property {number} windSpeedKt - The wind speed in knots.
 * @property {number} windGustKt - The wind gust in knots.
 * @property {number} visibilityM - The visibility in meters.
 * @property {number} qnh - The QNH pressure.
 * @property {number} verticalVisibilityFt - The vertical visibility in feet.
 * @property {string} wxString - The weather phenomena string.
 * @property {boolean} auto - Indicates if the METAR is automated.
 * @property {CloudLayer[]} cloudLayers - Array of cloud layers.
 * @property {string} rules - The flight rules.
 */

/**
 * @typedef {Object} Forecast
 * @property {string} forcastFromTime - The starting time of the forecast.
 * @property {string} forcastToTime - The ending time of the forecast.
 * @property {string} changeIndicator - The indicator of forecast change.
 * @property {string} becomingTime - The time when forecast is becoming.
 * @property {number} probability - The probability of the forecast.
 * @property {number} windDirectionDeg - The wind direction in degrees.
 * @property {number} windSpeedKt - The wind speed in knots.
 * @property {number} windGustKt - The wind gust in knots.
 * @property {number} visibilityM - The visibility in meters.
 * @property {number} verticalVisibilityFt - The vertical visibility in feet.
 * @property {string} wxString - The weather phenomena string.
 * @property {CloudLayer[]} cloudLayers - Array of cloud layers.
 */

/**
 * @typedef {Object} TafInfo
 * @property {number} id - The ID of the TAF information.
 * @property {string} rawTAF - The raw TAF data.
 * @property {string} icao - The ICAO code.
 * @property {string} issueTime - The issue time of the TAF.
 * @property {string} validFrom - The valid start time of the TAF.
 * @property {string} validTo - The valid end time of the TAF.
 * @property {string} remarks - Remarks of the TAF.
 * @property {Forecast[]} forecasts - Array of forecasts.
 */

/**
 * @typedef {Object} WeatherData
 * @property {MetarInfo} metar - The METAR information.
 * @property {TafInfo} taf - The TAF information.
 */



exports.unused = {};