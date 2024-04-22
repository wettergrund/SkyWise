<script>
	import { onMount } from "svelte";
	import { fade, fly } from "svelte/transition";
    import { from, to } from '../../stores/store'
    export let visible = false;

    onMount (() => visible = true);


    // $: localFrom = '';
    // $: localTo = '';


    	/** @type {import('./$types').ActionData} */
	export let form;

    const updateIcao = (e) => {
        const name = e.target.name;
        const value = e.target.value;

        if(name == "from" && value.length > 3)
        {
            from.update((n) => n = value)
        } 
        else if(name == "to" && value.length > 3)
        {

            to.update((n) => n = value)
        }         
        
        console.log(name);
    };

    

</script>



<div>
<div in:fade >
    
    <form method="POST" action="?/get">
        
        <!-- // From -->
        <label>Departure
            <input name="from" type="text" on:input={updateIcao}/>
        </label>
        
        
        <!-- // To -->
        <label>Arrival
            <input name="to" type="text"  on:input={updateIcao} />
        </label>
        <button type="submit">Hej</button>
        
    </form>
    
</div>


{#if $from.length > 0 && $to.length > 3}
<h2>
    {
        `${$from.toUpperCase()} - ${$to.toUpperCase()}`
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

</div>

<style>
    div{
        background-color: antiquewhite;
   

     
    }
</style>