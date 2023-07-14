#version 330 core
in vec2 fUv;

//uniform sampler2D uTexture0;
uniform vec3 color;

out vec4 FragColor;

void main()
{
    //FragColor = texture(uTexture0, fUv);
    FragColor = vec4(color, 1.0f);
    
}