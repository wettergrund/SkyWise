<script>
	import { onMount } from 'svelte';
	import { from, to } from '../../stores/store';
	import { Button, Modal, Label, Input, Checkbox } from 'flowbite-svelte';
  import { P, Heading } from 'flowbite-svelte';
	import WxTimeline from '../../components/WxTimeline.svelte';
	export let visible = false;

	onMount(() => (visible = true));

	/** @type {import('./$types').ActionData} */
	export let form;
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
</script>
<div class="header">
<Heading size="7xl">Search:</Heading>


<div class="form-wrapper">
	<div class="">
		<form method="POST" action="?/get" class="m-2 flex flex-col gap-5 space-y-0">
			<!-- // From -->
      <div class="flex flex-row gap-5">


			<Input size="lg" name="from" placeholder="Departure" type="text" on:input={updateIcao} />

			<!-- // To -->
			<Input size="lg" name="to" placeholder="Arrival" type="text" on:input={updateIcao} />
      </div>
			<Button disabled={$from.length === 0} type="submit">Get weather</Button>
		</form>
	</div>

	{#if $from.length > 0 && $to.length > 3}
		<h2>
			{`${$from.toUpperCase()} - ${$to.toUpperCase()}`}
		</h2>
	{/if}

	{#if form?.success}
		<WxTimeline metar={form?.from.metar}/>
		<br />
		<!-- {form?.from?.taf?.rawTAF ?? ""} -->

		<br />
		<WxTimeline metar={form?.to?.metar}/>

		<br />
		<!-- {form?.to?.taf.rawTAF ?? ""} -->
	{/if}
</div>
</div>


<style>
 .header{
   position: relative;
   background-color: #EBF0FF;
   height: 30vh;
 }
 .form-wrapper{
   position: absolute;
   background: #fff;
   bottom: 0;
   padding: 1rem 1rem;
   border-radius: 1.5rem 1.5rem 0 0;
   width: 100%;
 }
</style>
