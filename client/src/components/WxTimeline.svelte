<script>
    import { Timeline, TimelineItem, Button, Badge } from 'flowbite-svelte';
    import { ArrowRightOutline } from 'flowbite-svelte-icons';

  
    /** @type {import('$lib/weatherTypes').MetarInfo | null} */
    export let metar;
  </script>
  

  {#if metar?.rules == "VFR"}
    <Badge border color="green">VFR</Badge>
    {:else if metar?.rules.endsWith("IFR")}
    <Badge border color="red">IFR</Badge>
    {:else if metar?.rules == "MVFR"}
    <Badge border color="blue">MVFR</Badge>
  {/if} 