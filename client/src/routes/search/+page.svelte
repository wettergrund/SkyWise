<script>
	import { onMount } from "svelte";
	import { fade, fly } from "svelte/transition";
    export let visible = false;

    onMount (() => visible = true);

    $: from = '';
    $: to = '';


    	/** @type {import('./$types').ActionData} */
	export let form;

    

</script>


{#if visible}

<dir in:fade >
    
    <form method="POST" action="?/get">
        
        <!-- // From -->
        <label>Departure
            <input name="from" type="text" bind:value={from} />
        </label>
        
        
        <!-- // To -->
        <label>Arrival
            <input name="to" type="text" bind:value={to}/>
        </label>
        <button type="submit">Hej</button>
        
    </form>
    
</dir>
{/if}

{#if from.length > 0 && to.length > 3}
<h2>
    {
        `${from.toUpperCase()} - ${to.toUpperCase()}`
    }
</h2>

{/if}


{#if form?.success}

{
    form?.from.metar.rawMetar
}
<br/>
{
    form?.from.taf.rawTAF
}

<br/>


{
    form?.to?.metar.rawMetar
}
<br/>
{
    form?.to?.taf.rawTAF
}
{/if}