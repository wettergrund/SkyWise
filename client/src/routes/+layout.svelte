<script>
    import { from, to } from '../stores/store'

  // your script goes here
</script>
<style lang="scss">
  @import '../style.scss';
  

  div {
    background-color:#cddefa;
  }
</style>
<div>


<h1>SkyWise</h1>


<p>{$from.toUpperCase()}</p>
<p>{$to.toUpperCase()}</p>

<p>Navbar</p>

</div>
<slot></slot>

