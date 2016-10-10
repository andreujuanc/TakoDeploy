module.exports = function(grunt) {
  // configure the tasks
  grunt.initConfig({
    //  Jade
    pug: {
      compile: {
        options: {
          pretty: true,
          data: {
            debug: false
          }
        },
        files: {
          "index.html": "jade/index.pug",
          "about.html": "jade/about.pug",
          "sources.html": "jade/sources.pug",
        }
      }
    } 
   
  });//initConfig

  grunt.loadNpmTasks('grunt-contrib-pug');

  grunt.registerTask('default', ['pug']);
}; //module export
