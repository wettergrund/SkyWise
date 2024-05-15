<script>
	import { onMount } from 'svelte';
	import { from, to } from '../../stores/store';
	import { Button, Modal, Label, Input, Checkbox, Toggle } from 'flowbite-svelte';
  import { P, Heading, Timeline } from 'flowbite-svelte';
	import WxTimeline from '../../components/WxTimeline.svelte';
	export let visible = false;

	onMount(() => (visible = true));

	/** @type {import('./$types').ActionData} */
	export let form;

    /** @type {boolean} */
    let localFlight = false;

	/**
	 * @param  {Event} e -
	 */
	const updateIcao = (e) => {
		if (!e.target) {
			return;
		}

		/**
		 * @type {HTMLInputElement}*/
		const target = e.target;

		const name = target.name;
		const value = target.value;

		if (name == 'from' && value.length > 3) {
			from.update((n) => (n = value));
		} else if (name == 'to' && value.length > 3) {
			to.update((n) => (n = value));
		}

		console.log(name);
	};

    const toggleLocalFlight = () => localFlight = !localStorage;
</script>
<div class="header">



<Timeline class="ml-5">

	
	{#if form?.success}
	
	{#if form.line != null}
	

	{#each form.line as weather, i}
	
	{#if i == 0}
	
		<WxTimeline weather={weather.weather} firstlast/>
	{/if}

	{#if i == form.line.length - 1}
		<WxTimeline weather={weather.weather} firstlast/>
	{/if}

	{#if i != form.line.length - 1 && i != 0}
		<WxTimeline weather={weather.weather} firstlast={false} />
	{/if}






		
	{/each}

	
	
	{:else if form.line == null && form.from?.metar != undefined}
	
	Single?
	
	<WxTimeline weather={form.from} firstlast/>
	
	{/if}
	
	
	{/if}
</Timeline>
	









<div class="form-wrapper">
	<div class="">
		<form method="POST" action="?/get" class="m-2 flex flex-col gap-5 space-y-0">
        <Toggle bind:checked={localFlight} on:click={toggleLocalFlight}>Local Flight</Toggle>
			<!-- // From -->
      <div class="flex flex-row gap-5">


			<Input size="lg" name="from" placeholder="Departure" type="text" on:input={updateIcao} />

			<!-- // To -->
			<Input disabled={localFlight} size="lg" name="to" placeholder="Arrival" type="text" on:input={updateIcao} />
      </div>
			<Button disabled={$from.length === 0 || (!localFlight && $to.length == 0 )  } type="submit">Get weather</Button>
		</form>
	</div>

	{#if $from.length > 0 && $to.length > 3}
		<h2>
			{`${$from.toUpperCase()} - ${$to.toUpperCase()}`}
		</h2>
	{/if}
</div>
</div>


<style>
 .header{
   position: relative;
   background-color: #f9fafb ;
   min-height: 30vh;
 }
 .form-wrapper{
   position: relative; 
   background: #fff;
   bottom: 0;
   padding: 1rem 1rem;
   border-radius: 1.5rem 1.5rem 0 0;
   width: 100%;
 }
</style>
