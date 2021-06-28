<template>
  <div>
    <transition name="bounce">

      <div v-if="isVisible" class="hidingWindow d-flex flex-column justify-content-center align-items-center">

        <div class="spinner spinner-border text-white" role="status">
        </div>

        <p class="waiting-text text-white">
            Please wait while your site is will be crawled
        </p>

      </div>

    </transition>
  </div>
</template>

<script>
  export default {
    name: 'hidingWindow',
    props:
    {
      isVisible: {
        default: false,
      },
    }
  }
</script>

<style>
  .hidingWindow{
    position:fixed;
    top:0;
    left:0;
    width:100vw;
    height:100vh;
    background-color:gray;
  }

  .spinner{
    width:64px;
    height:64px;
  }

  .waiting-text{
    font-size:2rem;
  }

  .bounce-enter-active {
    animation: bounce-in 1s;
  }

  .bounce-leave-active {
    animation: bounce-in 1s reverse;
  }

  @keyframes bounce-in {
    0% {
      transform: scale(0);
    }

    50% {
      transform: scale(1.5);
    }

    100% {
      transform: scale(1);
    }
  }
</style>
