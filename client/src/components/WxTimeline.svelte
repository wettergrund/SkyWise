<script>
	import { Timeline, TimelineItem, Button, Badge, Card, P, Heading, Span, Hr, Accordion, AccordionItem, SidebarItem, Sidebar, SidebarWrapper, SidebarDropdownWrapper } from 'flowbite-svelte';
	import {
		ArrowRightOutline,
		CalendarWeekSolid,
		MapPinSolid,
		MapPinAltSolid,
		DotsVerticalOutline
	} from 'flowbite-svelte-icons';
	import { WindIcon } from 'svelte-feather-icons';
	import RuleBadge from './WeatherComponents/RuleBadge.svelte';
	import MetarCard from './WeatherComponents/MetarCard.svelte';
	import { from, to } from '../stores/store';
	import airportData from 'airport-data-js'
	import RuleIndicator from './WeatherComponents/RuleIndicator.svelte';






	/** @type {import('$lib/weatherTypes').WeatherData} */
	export let weather;

    /** @type {boolean} */
	export let firstlast;


	    /** @type {string} */
		let airportName = "";



	airportData.getAirportByIcao(weather.metar.icao)
	.then(data => {
		try {
      const icao = data[0].icao;
      airportName = data[0].airport;
      console.log(icao);
      console.log(airportName);
    } catch (error) {
      console.error('Error accessing icao property:', error);
    }
	})
	.catch(error => {
    console.error('Error fetching airport data:', error);
  });


	const options = {
		year: 'numeric',
		month: 'long',
		day: 'numeric',
		hour: 'numeric',
		minute: 'numeric',
		hour12: false,
		timeZone: 'UTC',
		timeZoneName: 'short'
	};
	const local = {
		hour: 'numeric',
		minute: 'numeric',
		hour12: false,
		timeZoneName: 'short'
	};

	const zTime =  new Date(weather.metar?.validFrom + 'Z').toLocaleString('en-US', options);
	const localTime = new Date(weather.metar?.validFrom + 'Z').toLocaleString('en-US', local);
	
</script>
{#if firstlast}
	
<TimelineItem
date={`${zTime} (Local: ${localTime})`}
title={weather.metar?.icao + " " + airportName}
>
	<svelte:fragment slot="icon">
		<span
		class="absolute -start-3 flex h-6 w-6 items-center justify-center rounded-full bg-primary-200 ring-8 ring-white dark:bg-primary-900 dark:ring-gray-900"
		>
		<MapPinAltSolid class="h-4 w-4 text-primary-600 dark:text-primary-400" />
	</span>
</svelte:fragment>



<MetarCard metar={weather.metar}/>

{#if weather.taf != null}
<Hr classHr="my-8 w-64">Forecast</Hr>


{/if}

</TimelineItem>
{:else}
<TimelineItem>
	<svelte:fragment slot="icon">
		<span
		class="absolute -start-3 flex h-6 w-6 items-center justify-center rounded-full bg-primary-200 ring-8 ring-white dark:bg-primary-900 dark:ring-gray-900"
		>
		<DotsVerticalOutline class="h-4 w-4 text-primary-600 dark:text-primary-400" />
	</span>
</svelte:fragment>

<Sidebar class="w-6/12">
	<SidebarWrapper class="p-0">
		<SidebarDropdownWrapper class="p-0 miniWx" label={weather.metar.icao +  " " + airportName}>
			<svelte:fragment slot="icon">
<RuleIndicator rule={weather.metar.rules}/>
				
			  </svelte:fragment>

			<MetarCard metar={weather.metar}/>


			 

		</SidebarDropdownWrapper>

	</SidebarWrapper>

</Sidebar>
<!-- <Accordion flush>
	<AccordionItem>
		<span slot="header">My Header 1</span>
	</AccordionItem>
</Accordion> -->

</TimelineItem>


{/if}

