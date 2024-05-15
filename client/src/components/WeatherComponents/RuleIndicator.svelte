<script>
	import { Indicator } from "flowbite-svelte";

    /** @type {string} */
	export let rule;

    let color;

    switch(rule){
        case 'VFR':
            color = 'green'
            break;
        case 'IFR':
            color = 'red'
            break;
        case 'MVFR':
            color = 'blue'
            break;
            default:
                color= 'red'


        


    }


    
</script>

<!-- {#if rule == 'VFR'}
<Indicator color="green" />
{:else if rule.endsWith('IFR')}
<Indicator color="red" />
{:else if rule == 'MVFR'}
<Indicator color="blue" />
{/if} -->
<Indicator color={color} size="xs"/>