<script>
	import { onMount } from 'svelte';
	import { from, to } from '../../stores/store';
	import { Button, Modal, Label, Input, Checkbox } from 'flowbite-svelte';

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

<div class="form-wrapper">
	<div class="">
		<form method="POST" action="?/get" class="m-2 flex flex-col gap-5 space-y-0">
			<!-- // From -->
			<Input name="from" placeholder="Departure" type="text" on:input={updateIcao} />

			<!-- // To -->
			<Input name="to" placeholder="Arrival" type="text" on:input={updateIcao} />
			<Button type="submit">Get weather</Button>
		</form>
	</div>

	{#if $from.length > 0 && $to.length > 3}
		<h2>
			{`${$from.toUpperCase()} - ${$to.toUpperCase()}`}
		</h2>
	{/if}

	{#if form?.success}
		{form?.from.metar.rules}
		<br />
		{form?.from.taf.rawTAF}

		<br />

		{form?.to?.metar.rules}
		<br />
		{form?.to?.taf.rawTAF}
	{/if}
</div>

<style>
	.Input {
		
        text-transform: uppercase;
	}
	.form > form {
		display: flex;
		flex-direction: column;
		gap: 1rem;
		padding: 1rem;
	}
	form > input {
		border: 1px solid #a0a0a0;
		border-radius: 5px;
	}
</style>
