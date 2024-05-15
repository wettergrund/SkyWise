<script>
	import { Card, Heading, P, Span } from 'flowbite-svelte';
	import { WindIcon } from 'svelte-feather-icons';
	import RuleBadge from './RuleBadge.svelte';



	/** @type {import('$lib/weatherTypes').MetarInfo} */
	export let metar;



    
</script>


<RuleBadge rule={metar.rules} auto={metar.auto}/>
<Card padding="none" class="relative">
    
    
    <div class="grid grid-cols-2 card-top">
    <!-- <Span tag="h2" center>{metar.icao.toUpperCase()}</Span> -->


    <div class="wind-section flex flex-col items-center">

        <div>
            {`${metar.windDirectionDeg < 100 ? 0 : "" }${metar.windDirectionDeg}`}°
        </div>
        <div>
           
            {metar.windSpeedKt}kt
        </div>
        <div>
            {#if metar.windGustKt > 0}
            <div class="wind-gust flex text-primary-600">
                <WindIcon class="mr-3" />

                
                {metar.windGustKt}kt
            </div>
            {/if}
        </div>
    </div>
    <div class="cloud">s</div>
    </div>

    <!-- <Heading tag="h5">Additional Info</Heading> -->
    <!-- <div class="additional-info flex flex-wrap gap-3 justify-between">
        <div class="info grid grid-cols-2 gap-3">
            <div>Temp</div>
            <div>{metar.temp + '°C'}</div>
            <div>Dew</div>
            <div>{metar.dewPoint + '°C'}</div>
            {#each metar.cloudLayers as cloud }
            <div>{cloud.cover}</div>
            <div>{cloud.cloudBase}ft</div>
                
            {/each}
        </div>
        <div class="pressure bg-green-100  ">
        {metar.qnh}
        </div>
    </div> -->

    <div class="p-3">

    <div class="grid md:grid-cols-3">
     
        <figure>
            <Heading tag="h6">Cloud</Heading>
            <div class="info grid grid-cols-2 gap-3">
      
                {#each metar.cloudLayers as cloud }
                <P>{cloud.cover}</P>
                <div>{cloud.cloudBase}ft</div>
                    
                {/each}
            </div>
        </figure>
        <figure>
            <Heading tag="h6">Pressure</Heading>
            <P>{metar.qnh}</P>


        </figure>
           
            <figure>
                <Heading tag="h6">Temp</Heading>
                <div class="info grid grid-cols-2 gap-3">
                    <div>Temp</div>
                    <div>{metar.temp + '°C'}</div>
                    <div>Dew</div>
                    <div>{metar.dewPoint + '°C'}</div>
                   
                </div>
            </figure>

    </div>

    <div class="raw">

        <P weight="light">
            {metar.rawMetar}
        </P>
    </div>
</div>

</Card>

<style>
    .info{
    
    height: fit-content;
    }
    .info div{
        height: 1rem;
    }
    .pressure{
        display: flex;
        justify-content: center;
        align-items: center;
        height: 100px;
        width: 100px;
        border-radius: 50%;
    }
    .wind-section{
        /* background-color:#f8f8f8 ; */
        padding: 2rem;
        border-radius: .5rem;
    }
    .raw{
        /* background-color: rgb(197, 197, 197); */
        font-family: 'Courier New', Courier, monospace;
        margin-top: 2rem;
        
    }

    .additional > div{
        height: 100px;
        width: 100px;
        background-color: antiquewhite;
    }

    .card-top{
        background-image: linear-gradient(#f9f9f9, #f8f8f8);
    }
    </style>
    