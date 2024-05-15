<script>
	import { Badge } from "flowbite-svelte";

    /** @type {string} */
	export let rule;
    /** @type {boolean} */
	export let auto;



    
</script>

{#if rule == 'VFR'}
<Badge border color="green" >VFR</Badge>
{:else if rule.endsWith('IFR')}
<Badge border color="red" class="">IFR</Badge>
{:else if rule == 'MVFR'}
<Badge border color="blue" >MVFR</Badge>
{/if}
{#if auto}

	<Badge color="blue" >Auto</Badge>
{/if}


