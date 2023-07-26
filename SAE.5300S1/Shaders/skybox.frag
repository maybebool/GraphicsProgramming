#version 330 core
out vec4 FragColor;
//uniform vec3 color; 

uniform samplerCube skybox;
in vec3 fTexCoords;


void main()
{
    FragColor  = texture(skybox, fTexCoords);
    //FragColor = vec4(color * baseColor, 0.5f);
}