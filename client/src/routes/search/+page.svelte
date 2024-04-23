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



<div class="form-wrapper">
<div class="form">
    
    <form method="POST" action="?/get">
        
        <!-- // From -->
            <input name="from" placeholder="Departure" type="text" on:input={updateIcao}/>
        
        
        <!-- // To -->
            <input name="to" placeholder="Arrival" type="text"  on:input={updateIcao} />
        <button type="submit">Get weather</button>
        
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
    form?.from.metar.rules
}
<br/>
{
    form?.from.taf.rawTAF
}

<br/>


{
    form?.to?.metar.rules
}
<br/>
{
    form?.to?.taf.rawTAF
}
{/if}

</div>

<style>
  input{
    border: 0;
    height: 2.5rem;

  }
  .form > form{
    
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
